using System.Collections.Generic;
using CardDB.Updates;


namespace CardDB
{
	public class Card
	{
		private Dictionary<string, CardIndex>	m_indexByCardID	= new();
		private SortedSet<CardIndex>			m_view			= new();
		
		
		public string ID { get; set; }
		public string BucketID { get; set; }
		public ulong SequenceID { get; set; }
		public Dictionary<string, string> Properties { get; set; }
		public int Count => m_view.Count;
		public bool IsDeleted { get; set; } = false;
		public bool IsView => Indexer != null; 
		public IIndexer Indexer { get; set; }
		
		
		public void SetProperty(string key, string value) => Properties[key] = value;
		public void DeleteProperty(string key) => Properties.Remove(key);
		
		
		public void UpdateSequence(ulong to)
		{
			SequenceID = to > SequenceID ? to : SequenceID;
		}
		
		
		public void Delete()
		{
			IsDeleted = true;
			Properties.Clear();
		}
		
		
		#region Index Functions
		
		private IndexUpdate CardRemoved(ulong sequence, CardIndex index)
		{
			m_indexByCardID.Remove(index.CardID);
			m_view.Remove(index);
			
			return IndexUpdate.Removed(sequence, index);
		}
		
		private IndexUpdate CardAdded(ulong sequence, CardIndex index)
		{
			m_indexByCardID.Add(index.CardID, index);
			m_view.Add(index);
			
			return IndexUpdate.Added(sequence, index);
		}
		
		private IndexUpdate CardReIndexed(ulong sequence, CardIndex prev, CardIndex curr)
		{
			if (prev.CompareTo(curr) == 0)
				return null;
			
			m_indexByCardID[curr.CardID] = curr;
			m_view.Remove(prev);
			m_view.Add(curr);
			
			return IndexUpdate.ReIndexed(sequence, prev, curr);
		}
		
		
		public IndexUpdate Index(ulong sequence, Card c)
		{
			m_indexByCardID.TryGetValue(c.ID, out var existingIndex);
			var newOrderValue = Indexer.Index(c);
			
			// New Card
			if (newOrderValue != null && existingIndex == null)
			{
				return CardAdded(sequence, new CardIndex { Card = c, View = this, Order = newOrderValue });
			}
			// Removed card
			else if (newOrderValue == null && existingIndex != null)
			{
				return CardRemoved(sequence, existingIndex);
			}
			// Modified card
			else if (newOrderValue != null && existingIndex != null)
			{
				return CardReIndexed(
					sequence,
					existingIndex, 
					new CardIndex { Card = c, View = this, Order = newOrderValue }
				);
			}
			
			return null;
		}
		
		public IndexUpdate Remove(ulong sequence, Card c)
		{
			m_indexByCardID.TryGetValue(c.ID, out var existingIndex);
			
			if (existingIndex == null)
			{
				return null;
			}
			
			return CardRemoved(sequence, existingIndex);
		}
		
		public CardIndex[] GetList(int count, CardIndex after)
		{
			List<CardIndex> result = new();

			foreach (var index in m_view)
			{
				if (index.Card.IsDeleted)
					continue;
				
				if (index.CompareTo(after) > 0)
				{
					result.Add(index);
					
					if (count <= result.Count)
					{
						break;
					}
				}
			}
			
			return result.ToArray();
		}
		
		#endregion
	}
}