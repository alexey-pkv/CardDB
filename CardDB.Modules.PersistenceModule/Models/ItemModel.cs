using System;
using System.Collections.Generic;
using CardDB.Modules.PersistenceModule.Base;

namespace CardDB.Modules.PersistenceModule.Models
{
	public class ItemModel : IDataModel<ItemModel>
	{
		public string PrimaryID => "ID";
		
		
		private Card m_card;
		private View m_view;
		
		
		public ItemModel() {}
		public ItemModel(Card card) { m_card = card; }
		public ItemModel(View view) { m_view = view; }
		
		
		public void From(ItemModel obj)
		{
			throw new System.NotImplementedException();
		}

		public void From(Dictionary<string, object> data)
		{
			throw new System.NotImplementedException();
		}

		public Dictionary<string, object> ToData()
		{
			throw new System.NotImplementedException();
		}

		public ItemModel ToObject()
		{
			throw new System.NotImplementedException();
		}
		

		public void SetAutoIncID(long val)
		{
			throw new NotSupportedException();
		}
	}
}