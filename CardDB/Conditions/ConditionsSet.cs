using System.Collections.Generic;


namespace CardDB.Conditions
{
	public abstract class ConditionsSet : ICondition
	{
		public List<ICondition> Children { get; set; } = new();
		
		
		public abstract bool IsMatching(Card card);
	}
}