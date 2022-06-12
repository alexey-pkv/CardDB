using System.Collections.Generic;

namespace CardDB.Modules.APIModule.Models
{
	public class CardIndexModel
	{
		public CardModel card { get; }
		public object[] order { get; }
		
		
		public CardIndexModel(CardIndex index)
		{
			card = new CardModel(index.Card);
			
			var list = new List<object>(index.Order.Value) { index.Card.ID };
			
			order = list.ToArray();
		}
	}
}