namespace CardDB
{
	public interface IIndexer
	{
		public OrderValue Index(Card card);
	}
}