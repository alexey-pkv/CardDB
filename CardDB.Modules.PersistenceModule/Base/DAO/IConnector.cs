using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface IConnector
	{
		public Task Test();
		
		public IActionDAO Action { get; }
	}
}