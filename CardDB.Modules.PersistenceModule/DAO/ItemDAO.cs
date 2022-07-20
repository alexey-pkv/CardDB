using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base.DAO;
using CardDB.Modules.PersistenceModule.Models;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class ItemDAO : IItemDAO
	{
		private const string TABLE	= "Item";
		
		
		private IMySQLConnectionProvider m_provider;
		
		
		public ItemDAO(IMySQLConnectionProvider connectionProvider)
		{
			m_provider = connectionProvider;
		}
		
		
		public async Task Insert(Card c)
		{
			await m_provider.Insert(TABLE, new ItemModel(c));
		}

		public Task Update(Card c)
		{
			throw new System.NotImplementedException();
		}
	}
}