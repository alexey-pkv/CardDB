using System.Collections.Generic;
using System.Linq;


namespace CardDB
{
	public class CardsSet
	{
		public Dictionary<string, Card> Cards { get; } = new();
		public int Count => Cards.Count;
		
		
		public IEnumerable<Card> GetCards()
		{
			lock (Cards)
			{
				return Cards.Values.ToArray();
			}
		}

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