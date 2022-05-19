using System.Collections.Generic;


namespace CardDB
{
	public class ViewsSet
	{
		public Dictionary<string, View> Views { get; } = new();
		
		
		public IEnumerable<View> GetViews()
		{
			lock (Views)
			{
				return Views.Values;
			}
		}
		
		
		public void AddView(View view)
		{
			lock (Views)
			{
				Views.Add(view.ID, view);
			}
		}
		
		public View RemoveView(string id)
		{
			View v = null;
			
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