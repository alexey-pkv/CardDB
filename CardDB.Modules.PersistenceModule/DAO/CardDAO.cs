using System;
using System.Collections.Generic;
using System.Linq;
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

		public async Task Update(Card c)
		{
			await m_provider.Update(TABLE, new CardModel(c));
		}
		
		public async Task UpdateAll(IEnumerable<Card> c)
		{
			var models = c.Select(c => new CardModel(c));
			await m_provider.UpdateAll(TABLE, models);
		}
		
		public async Task<Card> Load(string id)
		{
			return await m_provider.LoadOneByField<CardModel, Card>(TABLE, "ID", id);
		}
		
		public async Task<Card[]> LoadAll(string after = null)
		{
			var where	= (after == null ? "TRUE" : "ID > ?");
			var bind	= (after == null ? new object[] {} : new object[] { after });
			var cmd			= $"SELECT * FROM {TABLE} WHERE {where} ORDER BY ID LIMIT 100";
			
			return await m_provider.Select<CardModel, Card>(cmd, bind);
		}
		
		public async Task LoadAll(Func<Card[], Task> callback)
		{
			Card[] c = await LoadAll();
			
			while (c.Any())
			{
				await callback(c);
				c = await LoadAll(c.Last().ID);
			}
		}
	}
}