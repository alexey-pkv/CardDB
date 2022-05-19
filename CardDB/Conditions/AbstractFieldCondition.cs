namespace CardDB.Conditions
{
	public abstract class AbstractFieldCondition : ICondition
	{
		public string Field { get; set; }
		
		
		public abstract bool IsMatching(Card card);
		
		
		public AbstractFieldCondition() {}
		public AbstractFieldCondition(string field) 
		{
			Field = field;
		}
	}
}