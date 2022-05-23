using Library;
using CardDB.Engine;
using Library.State;


namespace CardDB.Modules.DBModule
{
	public class DBModule : AbstractModule, IDBModule
	{
		public override string Name => "DB";
		public DBEngine Engine { get; } = new ();


		public override void Init()
		{
			Engine.Start();
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