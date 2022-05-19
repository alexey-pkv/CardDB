namespace CardDB.Conditions.ValueConditions
{
	public class IsValueCondition : AbstractValueCondition
	{
		public override bool IsMatching(Card card)
		{
			if (!card.Properties.TryGetValue(Field, out var val))
			{
				return false;
			}
			
			return val == Value;
		}
	}
}