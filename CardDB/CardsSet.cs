using System.Collections.Generic;
using Library.ID;


namespace CardDB
{
	public class CardsSet
	{
		public Dictionary<string, Card> Cards { get; } = new();
		
		
		public void AddCard(Card card)
		{
			lock (Cards)
			{
				Cards[card.ID] = card;
			}
		}
		
		
		public void RemoveCard(Card card) => RemoveCard(card.ID);
		public void RemoveCard(string id)
		{
			lock (Cards)
			{
				Cards.Remove(id);
			}
		}
	}
}