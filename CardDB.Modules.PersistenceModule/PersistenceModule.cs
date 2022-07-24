using System;
using System.Threading.Tasks;
using AdaptiveExpressions;
using CardDB.MySQL;
using CardDB.Modules.PersistenceModule.DAO;
using CardDB.Modules.PersistenceModule.Exceptions;
using CardDB.Updates;
using Library;
using Library.ID;
using Library.State;
using Library.Tasks;


namespace CardDB.Modules.PersistenceModule
{
	public class PersistenceModule : AbstractModule, IPersistenceModule
	{
		public override string Name => "Persistence";

		
		private Connector m_connector;
		private SimpleTaskQueue m_queue = new();
		
		
		private LRUCache<string, Bucket> m_bucketByName = new(1000);
		private LRUCache<string, Bucket> m_bucketByID = new(1000);
		
		
		private async Task SetupDB()
		{
			var commands = await DatabaseSetup.GetCreateDB();
			var connection = await m_connector.GetBare();

			foreach (var command in commands)
			{
				var cmd = connection.CreateCommand();
				
				cmd.CommandText = command;
				await cmd.ExecuteNonQueryAsync();
			}
		}
		
		
		private async Task PreLoadAsync(bool setupDB)
		{
			if (setupDB)
			{
				await SetupDB();
			}
			
			try
			{
				m_connector.Test().Wait();
				Log.Info($"[{Name}] Connection to DB: OK");
			}
			catch (Exception e)
			{
				Log.Fatal($"[{Name}] Connection to DB: FAILED!!!", e);
				throw;
			}
		}
		
		
		public override void Init()
		{
			base.Init();
			
			m_connector = new Connector(Config);
		}
		
		
		public override void PreLoad(IStateManager state)
		{
			state.CompleteAfter(PreLoadAsync, param: true);
		}
		
		
		#region Action Persistence
		
		public Task Persist(Action a) => Persist(a, null);

		public Task Persist(Action a, Action<Action> callback)
		{
			return m_queue.EnqueueAndWait(async () =>
			{
				await m_connector.Action.Save(a);
				callback(a);
			});
		}
		
		#endregion
		
		#region Updates 
		
		public void Consume(IUpdate update)
		{
			if (update.TargetType == UpdateTarget.Card && 
			    update.UpdateType == UpdateType.Added)
			{
				m_connector.Card.Insert(((CardUpdate)update).Card);
			}
		}
		
		#endregion

		#region Bucket


		public async Task<Bucket> Create(string name)
		{
			name = Formats.SanitizeBucketName(name);
			
			if (m_bucketByName.TryGet(name, out _))
				throw new BucketAlreadyExistsException(name);
			
			Bucket bucket = new Bucket
			{
				ID		= await IDGenerator.Generate(),
				Name	= name
			};
			
			if (!await m_connector.Bucket.Save(bucket))
			{
				throw new BucketAlreadyExistsException(name);
			}
			
			m_bucketByName.Set(bucket.Name, bucket);
			m_bucketByID.Set(bucket.ID, bucket);
			
			return bucket;
		}

		public async Task<Bucket> GetByName(string name)
		{
			Bucket bucket;
			
			name = Formats.SanitizeBucketName(name);
			
			if (m_bucketByName.TryGet(name, out bucket))
				return bucket;
			
			bucket = await m_connector.Bucket.LoadByName(name);
			
			if (bucket != null)
			{
				m_bucketByName.Set(bucket.Name, bucket);
				m_bucketByID.Set(bucket.ID, bucket);
			}
			
			return bucket;
		}
		
		#endregion
	}
}