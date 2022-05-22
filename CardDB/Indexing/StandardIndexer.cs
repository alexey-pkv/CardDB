using System.Collections.Generic;


namespace CardDB.Indexing
{
	public class StandardIndexer : IIndexer
	{
		public ICondition Condition { get; init; }
		public string[] OrderProperties { get; init; }
		
		
		public OrderValue Index(Card card)
		{
			if (!Condition.IsMatching(card))
				return null;
			
			List<object> orderList = new(OrderProperties.Length);

			foreach (var propertyName in OrderProperties)
			{
				if (!card.Properties.TryGetValue(propertyName, out var propertyValue))
				{
					orderList.Add(null);
				}
				else
				{
					orderList.Add(propertyValue);
				}
			}
			
			return new OrderValue(orderList);
		}
	}
}