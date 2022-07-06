using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface IConnector
	{
		public Task Test();
		public Task CreateDB();
		
		public IActionDAO Action { get; }
	}
}