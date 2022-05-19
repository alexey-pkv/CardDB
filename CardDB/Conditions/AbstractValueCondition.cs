namespace CardDB.Conditions
{
	public abstract class AbstractValueCondition : AbstractFieldCondition
	{
		public string Value { get; set; }
		
		
		public AbstractValueCondition() {}
		public AbstractValueCondition(string field, string value) : base(field) 
		{
			Value = value;
		}
	}
}