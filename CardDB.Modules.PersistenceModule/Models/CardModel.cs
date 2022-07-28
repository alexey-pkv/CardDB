using System;
using System.Collections.Generic;

using CardDB.Modules.PersistenceModule.Base;
using CardDB.Modules.PersistenceModule.Models.CardParts;


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
			m_card = obj;
		}
		
		public void From(Dictionary<string, object> data)
		{
			m_card = new Card();
			
			if (data.TryGetValue("ID", out var id))
				m_card.ID = id.ToString();
			
			if (data.TryGetValue("BucketID", out var bid))
				m_card.BucketID = bid.ToString();
			
			if (data.TryGetValue("SequenceID", out var sid))
				m_card.SequenceID = ulong.Parse(sid.ToString() ?? 
					throw new InvalidOperationException("Invalid sid " + sid));
			
			if (data.TryGetValue("IsDeleted", out var is_del))
				m_card.IsDeleted = is_del.ToString() == "1";
			
			if (data.TryGetValue("Data", out var d))
			{
				CardData.SetData(d.ToString(), m_card);
			}
			else
			{
				m_card.Properties = new Dictionary<string, string>();
			}
		}
		
		public Dictionary<string, object> ToData()
		{
			return new Dictionary<string, object>
			{
				{ "ID",			m_card.ID },
				{ "BucketID",	m_card.BucketID },
				{ "SequenceID",	m_card.SequenceID },
				{ "IsDeleted",	m_card.IsDeleted },
				{ "Type",		m_card.IsView ? "View" : "Card" },
				{ "Data",		CardData.GetData(m_card) }
			};
		}
		
		public Dictionary<string, object> ToUpdateData()
		{
			return new Dictionary<string, object>
			{
				{ "ID",			m_card.ID },
				{ "SequenceID",	m_card.SequenceID },
				{ "IsDeleted",	m_card.IsDeleted },
				{ "Type",		m_card.IsView ? "View" : "Card" },
				{ "Data",		CardData.GetData(m_card) }
			};
		}
		
		public Card GetObject()
		{
			return m_card;
		}
		
		
		public void SetAutoIncID(long val) {}
	}
}