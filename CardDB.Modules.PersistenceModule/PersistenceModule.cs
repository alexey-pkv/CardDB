using System;
using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.DAO;
using CardDB.MySQL;
using Library;
using Library.State;
using Library.Tasks;


namespace CardDB.Modules.PersistenceModule
{
	public class PersistenceModule : AbstractModule, IPersistenceModule
	{
		public override string Name => "Persistence";

		
		private Connector m_connector;
		private SimpleTaskQueue m_queue = new();
		
		
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
		
		
		private async Task PreLoadAsync(IStateToken token, bool setupDB)
		{
			try
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
				
				token.Complete();
			}
			catch (Exception e)
			{
				token.Fail(e);
			}
		}
		
		private async Task PersistSync(Action a, Action<Action> callback, TaskCompletionSource source)
		{
			await m_connector.Action.Save(a);
			callback(a);
			source.SetResult();
		}
		
		private async Task ExecutePersist(Action a, Action<Action> callback, TaskCompletionSource source)
		{
			
		}
		
		
		public override void Init()
		{
			base.Init();
			
			m_connector = new Connector(Config);
		}
		
		
		public override void PreLoad(IStateManager state)
		{
			var token = state.CreateToken();
			
			Task.Run(() => PreLoadAsync(token, true));
		}
		
		
		


		public Task Persist(Action a) => Persist(a, null);

		public Task Persist(Action a, Action<Action> callback)
		{
			return m_queue.EnqueueAndWait(async () =>
			{
				await m_connector.Action.Save(a);
				callback(a);
			});
		}
	}
}