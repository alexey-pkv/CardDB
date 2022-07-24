using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base.DAO;
using CardDB.Modules.PersistenceModule.Models;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class CardDAO : ICardDAO
	{
		private const string TABLE	= "Card";
		
		
		private IMySQLConnectionProvider m_provider;
		
		
		public CardDAO(IMySQLConnectionProvider connectionProvider)
		{
			m_provider = connectionProvider;
		}
		
		
		public async Task Insert(Card c)
		{
			await m_provider.Insert(TABLE, new CardModel(c));
		}

		public Task Update(Card c)
		{
			throw new System.NotImplementedException();
		}
	}
}