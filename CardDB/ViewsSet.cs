using System.Collections.Generic;


namespace CardDB
{
	public class ViewsSet
	{
		public Dictionary<string, Card> Views { get; } = new();
		public int Count => Views.Count;
		
		
		public IEnumerable<Card> GetViews()
		{
			lock (Views)
			{
				return Views.Values;
			}
		}
		
		
		public void AddView(Card view)
		{
			lock (Views)
			{
				Views.Add(view.ID, view);
			}
		}
		
		public Card RemoveView(string id)
		{
			Card v = null;
			
			lock (Views)
			{
				if (Views.TryGetValue(id, out v))
				{
					Views.Remove(id);
				}
			}
			
			return v;
		}
	}
}