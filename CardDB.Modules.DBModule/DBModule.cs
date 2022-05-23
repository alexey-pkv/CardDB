using Library;
using CardDB.Engine;


namespace CardDB.Modules.DBModule
{
	public class DBModule : AbstractModule, IDBModule
	{
		public override string Name => "DB";
		public DBEngine Engine { get; } = new ();
	}
}