using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
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
		private const int		BUFFER_SIZE			= 1000;
		private const double	STORE_INTERVAL_SEC	= 5;
		
		
		public override string Name => "Persistence";

		
		private Connector m_connector;
		private SimpleTaskQueue m_queue = new();
		
		
		private LRUCache<string, Bucket>	m_bucketByName = new(1000);
		private LRUCache<string, Bucket>	m_bucketByID = new(1000);
		private Dictionary<string, Card>	m_modified = new();
		
		private Timer m_storeTimer = new(STORE_INTERVAL_SEC * 1000);
		private object m_storeLock = new();
		private object m_modifiedLock = new();
		private bool m_isStoring = false;
		
		
		private async Task ExecuteStore()
		{
			IEnumerable<Card> modified;
			
			lock (m_modifiedLock)
			{
				modified = m_modified.Values.ToArray();
				m_modified.Clear();	
			}
			
			if (modified.Any())
			{
				var start = DateTime.Now;
				{
					await m_connector.Card.UpdateAll(modified);
				}
				var end = DateTime.Now;
				
				Log.Info($"[{Name}] Stored {modified.Count()} items in {end.Subtract(start).TotalMilliseconds} ms");
			}
		}
		
		private async Task Store()
		{
			try
			{
				await ExecuteStore();
			}
			catch (Exception e)
			{
				Log.Error($"[{Name}] Failed to store modified cards", e);
			}
			finally
			{
				lock (m_storeLock)
				{
					m_isStoring = false;
				}
			}
		}
		
		private void ScheduleStore()
		{
			lock (m_modifiedLock)
			{
				if (m_modified.Count == 0)
				{
					return;
				}
			}
			
			lock (m_storeLock)
			{
				if (m_isStoring)
					return;
				
				m_isStoring = true;
			}
			
			Task.Run(Store);
		}
		
		private void CheckShouldStore()
		{
			if (m_isStoring)
				return;
			
			lock (m_modifiedLock)
			{
				if (m_modified.Count < BUFFER_SIZE)
					return;
			}
			
			lock (m_storeLock)
			{
				if (m_isStoring)
					return;
				
				m_isStoring = true;
			}
			
			Task.Run(Store);
		}
		
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
			
			m_storeTimer.Elapsed += (_,_) => ScheduleStore();  
			m_storeTimer.Start();
		}

		public override void PreStop(IStateManager state)
		{
			m_storeTimer.Stop();
		}

		public override void Stop(IStateManager state)
		{
			state.CompleteAfter(ExecuteStore);
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
			if (update.TargetType != UpdateTarget.Card)
				return;
			
			var cardUpdate = update as CardUpdate;
			var card = cardUpdate.Card;
			var id = card.ID;
			var check = false;
			
			lock (m_modifiedLock)
			{
				m_modified.Add(id, card);
				
				check = (m_modified.Count >= BUFFER_SIZE);
			}
			
			if (check)
			{
				CheckShouldStore();
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