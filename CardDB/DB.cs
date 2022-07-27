namespace CardDB
{
	public class DB
	{
		public Bucket Bucket { get; }
		public CardsSet Cards { get; } = new();
		public ViewsSet Views { get; } = new();
		
		
		public DB(Bucket b)
		{
			Bucket = b;
		}
		
		
		public bool TryGetView(string id, out Card view)
		{
			return Views.Views.TryGetValue(id, out view);
		}
	}
}