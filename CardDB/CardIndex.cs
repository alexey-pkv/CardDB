using System;
using System.Collections.Generic;


namespace CardDB
{
	public class CardIndex : IComparable<CardIndex>
	{
		public Card Card { get; init; }
		public View View { get; init; }
		public OrderValue Order { get; init; }
		
		public string CardID => Card.ID;
		public string ViewID => Card.ID;
		
		
		public int CompareTo(CardIndex other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			
			var result = Order.CompareTo(other.Order);
			
			if (result == 0)
			{
				result = String.Compare(CardID, other.CardID, StringComparison.Ordinal);
				
				if (result < -1) result = -1;
				else if (result > 1) result = 1;
			}
			
			return result;
		}

		public override int GetHashCode()
		{
			return Order.GetHashCode() ^ Card.GetHashCode();
		}
	}
}