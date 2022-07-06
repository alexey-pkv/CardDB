using System.Collections.Generic;
using CardDB.Modules.PersistenceModule.Base.DAO;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class ActionDAO : IActionDAO
	{
		private IMySQLConnectionProvider m_provider;
		
		
		public ActionDAO(IMySQLConnectionProvider connectionProvider)
		{
			m_provider = connectionProvider;
		}
		
		
		public void Save(Action action)
		{
			throw new System.NotImplementedException();
		}
		
		public IEnumerable<Action> Load(ulong first, int limit)
		{
			throw new System.NotImplementedException();
		}
	}
}