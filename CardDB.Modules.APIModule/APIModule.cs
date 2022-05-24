using System.Threading.Tasks;
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
			m_server = new Server(
				Config["server.host"].Get(),
				Config["server.port"].GetInt());
			
			m_server.StartAsync();
		}

		public override void PreStop(IStateManager state)
		{
			m_server.Stop();
		}
	}
}