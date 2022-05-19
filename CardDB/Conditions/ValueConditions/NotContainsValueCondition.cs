namespace CardDB.Conditions.ValueConditions
{
	public class NotContainsValueCondition : AbstractValueCondition
	{
		public override bool IsMatching(Card card)
		{
			if (!card.Properties.TryGetValue(Field, out var val))
			{
				return true;
			}
			
			return !val.Contains(Value);
		}
	}
}