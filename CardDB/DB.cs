namespace CardDB
{
	public class DB
	{
		public CardsSet Cards { get; } = new();
		public FieldsSet Fields { get; } = new();
	}
}