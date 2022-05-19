namespace CardDB
{
	public class DB
	{
		public CardsSet Cards { get; } = new();
		public ViewsSet Views { get; } = new();
	}
}