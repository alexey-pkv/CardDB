using System.Collections.Generic;


namespace CardDB
{
	public class View : AbstractDBEntity
	{
		private Dictionary<string, CardIndex>	m_indexByCardID	= new();
		private SortedSet<CardIndex>			m_view			= new();
		private IIndexer						m_indexer		= null;
		
		
		public int Count => m_view.Count;
		
		
		private IndexAction CardRemoved(CardIndex index)
		{
			m_indexByCardID.Remove(index.CardID);
			m_view.Remove(index);
			
			return IndexAction.Removed(index);
		}
		
		private IndexAction CardAdded(CardIndex index)
		{
			m_indexByCardID.Add(index.CardID, index);
			m_view.Add(index);
			
			return IndexAction.Added(index);
		}
		
		private IndexAction CardReIndexed(CardIndex prev, CardIndex curr)
		{
			if (prev.CompareTo(curr) == 0)
				return null;
			
			m_indexByCardID[curr.CardID] = curr;
			m_view.Remove(prev);
			m_view.Add(curr);
			
			return IndexAction.ReIndexed(prev, curr);
		}
		
		
		public View(IIndexer indexer)
		{
			m_indexer = indexer;
		}
		
		
		public IndexAction Index(Card c)
		{
			m_indexByCardID.TryGetValue(c.ID, out var existingIndex);
			var newOrderValue = m_indexer.Index(c);
			
			// New Card
			if (newOrderValue != null && existingIndex == null)
			{
				return CardAdded(new CardIndex { Card = c, View = this, Order = newOrderValue });
			}
			// Removed card
			else if (newOrderValue == null && existingIndex != null)
			{
				return CardRemoved(existingIndex);
			}
			// Modified card
			else if (newOrderValue != null && existingIndex != null)
			{
				return CardReIndexed(
					existingIndex, 
					new CardIndex { Card = c, View = this, Order = newOrderValue }
				);
			}
			
			return null;
		}
		
		public IndexAction Remove(Card c)
		{
			m_indexByCardID.TryGetValue(c.ID, out var existingIndex);
			
			if (existingIndex == null)
			{
				return null;
			}
			
			return CardRemoved(existingIndex);
		}
		
		
		public CardIndex[] GetList(int count, CardIndex after)
		{
			List<CardIndex> result = new();

			foreach (var index in m_view)
			{
				if (index.CompareTo(after) > 0)
				{
					result.Add(index);
					
					if (count >= result.Count)
					{
						break;
					}
				}
			}
			
			return result.ToArray();
		}
	}
}