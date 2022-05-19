using System.Collections.Generic;


namespace CardDB.Conditions
{
	public class AndCondition : ConditionsSet
	{
		public override bool IsMatching(Card card)
		{
			foreach (var child in Children)
			{
				if (child.IsMatching(card))
				{
					return false;
				}
			}
			
			return true;
		}
	}
}