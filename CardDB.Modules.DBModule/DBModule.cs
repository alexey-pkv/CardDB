using Library;
using Library.State;

using CardDB.Engine;
using CardDB.Engine.Core;


namespace CardDB.Modules.DBModule
{
	public class DBModule : AbstractModule, IDBModule
	{
		public override string Name => "DB";
		public DBEngine Engine { get; } = new ();
		

		public override void PreLoad(IStateManager state)
		{
			var logs = GetModule<IUpdatesLogModule>();
			var persist = GetModule<IPersistenceModule>();
			
			Engine.Start(
				actionConsumers:	new IUpdatesConsumer[]{ logs, persist },
				indexConsumers:		new IUpdatesConsumer[]{ logs },
				p:					persist);
		}

		public override void PreStop(IStateManager state)
		{
			var token = state.CreateToken();
			
			Engine
				.Stop()
				.ContinueWith(_ => token.Complete());
		}
	}
}