using CardDB.Engine;
using Library;


namespace CardDB.Modules
{
	public interface IDBModule : IModule
	{
		public DBEngine Engine { get; }
	}
}