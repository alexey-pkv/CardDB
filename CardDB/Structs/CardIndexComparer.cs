using System.Collections.Generic;

namespace CardDB.Structs
{
	public class CardIndexComparer : IComparer<CardIndex>
	{
		public static readonly CardIndexComparer Instance = new CardIndexComparer(); 
		
		
		public int Compare(CardIndex x, CardIndex y)
		{
			if (ReferenceEquals(x, y)) return 0;
			if (ReferenceEquals(null, y)) return 1;
			if (ReferenceEquals(null, x)) return -1;
			
			return x.CompareTo(y);
		}
		
		
		private CardIndexComparer() {}
	}
}