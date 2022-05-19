using System.Collections.Generic;

namespace CardDB
{
	public class ViewsSet
	{
		public IEnumerable<View> GetViews()
		{
			lock (this)
			{
				return new View[] { };
			}
		}
	}
}