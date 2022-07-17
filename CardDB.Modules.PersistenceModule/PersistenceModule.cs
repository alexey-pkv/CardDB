using System;
using System.Threading.Tasks;

using CardDB.MySQL;
using CardDB.Modules.PersistenceModule.DAO;

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
			
		}
		
		#endregion
	}
}