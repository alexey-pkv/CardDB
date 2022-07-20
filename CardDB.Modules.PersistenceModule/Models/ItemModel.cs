using System.Collections.Generic;

using CardDB.Modules.PersistenceModule.Base;
using CardDB.Modules.PersistenceModule.Models.ItemParts;

using Library;


namespace CardDB.Modules.PersistenceModule.Models
{
	public class ItemModel : IDataModel<Card>
	{
		public bool IsAutoInc => false;
		public string PrimaryID => "ID";
		
		
		private Card m_card;
		
		
		public ItemModel() {}
		public ItemModel(Card card) { m_card = card; }
		
		
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
				var props = m_card.IsDeleted ? null : JSON.Serialize(m_card.Properties);
				
				return new Dictionary<string, object>
				{
					{ "ID",			m_card.ID },
					{ "SequenceID",	m_card.SequenceID },
					{ "Type",		ItemType.Card },
					{ "IsDeleted",	m_card.IsDeleted },
					{ "Data",		props }
				};
			}
			else
			{
				return null;
			}
		}
		
		
		public void SetAutoIncID(long val) {}
	}
}