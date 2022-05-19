using System.Collections.Generic;


namespace CardDB.Conditions
{
	public class OrCondition : ConditionsSet
	{
		public override bool IsMatching(Card card)
		{
			foreach (var child in Children)
			{
				if (child.IsMatching(card))
				{
					return true;
				}
			}
			
			if (Children.Count == 0)
				return true;
			
			return false;
		}
	}
}