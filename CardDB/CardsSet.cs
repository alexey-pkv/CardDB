using System.Collections.Generic;
using Library.ID;


namespace CardDB
{
	public class CardsSet
	{
		public Dictionary<string, Card> Cards { get; } = new();
		
		
		public void AddCard(Card card)
		{
			card.ID = IDGenerator.Generate();
			Cards[card.ID] = card;
		}
		
		
		public void RemoveCard(Card card) => RemoveCard(card.ID);
		public void RemoveCard(string id) => Cards.Remove(id);
	}
}