namespace CardDB.Conditions
{
	public class NotCondition : ICondition
	{
		public ICondition Child { get; set; }
		
		
		public bool IsMatching(Card card)
		{
			var child = Child;
			
			if (child == null)
			{
				return false;
			}
			
			return !child.IsMatching(card); 
		}
	}
}