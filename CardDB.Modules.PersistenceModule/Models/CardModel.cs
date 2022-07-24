using System.Collections.Generic;

using CardDB.Modules.PersistenceModule.Base;
using CardDB.Modules.PersistenceModule.Models.CardParts;
using Library;


namespace CardDB.Modules.PersistenceModule.Models
{
	public class CardModel : IDataModel<Card>
	{
		public bool IsAutoInc => false;
		public string PrimaryID => "ID";
		
		
		private Card m_card;
		
		
		public CardModel() {}
		public CardModel(Card card) { m_card = card; }
		
		
		public void From(Card obj)
		{
			throw new System.NotImplementedException();
		}
		
		public void From(Dictionary<string, object> data)
		{
			throw new System.NotImplementedException();
		}
		
		public Dictionary<string, object> ToData()
		{
			if (m_card != null)
			{
				return new Dictionary<string, object>
				{
					{ "ID",			m_card.ID },
					{ "SequenceID",	m_card.SequenceID },
					{ "IsDeleted",	m_card.IsDeleted },
					{ "Data",		CardData.GetData(m_card) }
				};
			}
			else
			{
				return null;
			}
		}
		
		public Card GetObject()
		{
			return m_card;
		}
		
		
		public void SetAutoIncID(long val) {}
	}
}