namespace CardDB.Protocol
{
	public interface IListener
	{
		public void CardCreated(Card card);
		public void CardModified(Card card);
		public void CardDeleted(Card card);
		
		public void ViewCreated(Card view);
		public void ViewDeleted(Card view);
		
		public void CardIndexed(CardIndex index);
	}
}