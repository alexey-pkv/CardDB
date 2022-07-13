using System.Collections.Generic;
using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface IActionDAO
	{
		public Task Save(Action action);
		public IEnumerable<Action> Load(ulong first, int limit);
		public Action LoadByID(ulong id);
	}
}