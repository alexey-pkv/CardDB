using System.Collections.Generic;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface IActionDAO
	{
		public void Save(Action action);
		public IEnumerable<Action> Load(ulong first, int limit);
	}
}