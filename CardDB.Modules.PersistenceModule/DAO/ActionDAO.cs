using System.Collections.Generic;
using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base.DAO;
using CardDB.Modules.PersistenceModule.Models;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class ActionDAO : IActionDAO
	{
		private const string TABLE	= "Action";
		
		
		private IMySQLConnectionProvider m_provider;
		
		
		public ActionDAO(IMySQLConnectionProvider connectionProvider)
		{
			m_provider = connectionProvider;
		}
		
		
		public async Task Save(Action action)
		{
			await m_provider.Insert(TABLE, new ActionModel(action));
		}
		
		public IEnumerable<Action> Load(ulong first, int limit)
		{
			throw new System.NotImplementedException();
		}
	}
}