namespace CardDB.Protocol
{
	public interface IListener
	{
		public void CardCreated(Card card);
		public void CardModified(Card card);
		public void CardDeleted(Card card);
		
		public void ViewCreated(View view);
		public void ViewDeleted(View view);
		
		public void CardIndexed(CardIndex index);
	}
}