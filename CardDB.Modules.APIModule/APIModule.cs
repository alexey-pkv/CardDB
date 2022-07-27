using Library;
using Library.State;
using WatsonWebserver;


namespace CardDB.Modules.APIModule
{
	public class APIModule : AbstractModule, IAPIModule
	{
		public override string Name => "WebAPI";
		
		
		private Server m_server;
		
		
		public override void Load(IStateManager state)
		{
			var host = Config["server.host"].Get();
			var port = Config["server.port"].GetInt();
			
			
			m_server = new Server(host, port);
			
			m_server.Events.ExceptionEncountered += APIErrorHandler.Handle;
			m_server.StartAsync();
			
			Log.Info($"[APIModule] Running on {host}:{port}");
		}

		public override void PreStop(IStateManager state)
		{
			m_server.Stop();
		}
	}
}