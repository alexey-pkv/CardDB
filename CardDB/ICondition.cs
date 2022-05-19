namespace CardDB
{
	public interface ICondition
	{
		public bool IsMatching(Card card);
	}
}