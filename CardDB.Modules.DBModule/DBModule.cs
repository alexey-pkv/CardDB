using Library;
using Library.State;

using CardDB.Engine;


namespace CardDB.Modules.DBModule
{
	public class DBModule : AbstractModule, IDBModule
	{
		public override string Name => "DB";
		public DBEngine Engine { get; } = new ();
		

		public override void PreLoad(IStateManager state)
		{
			var logs = GetModule<IUpdatesLogModule>();
			
			Engine.Start(logs);
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