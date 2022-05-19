namespace CardDB.Conditions.ValueConditions
{
	public class IsNotValueCondition : AbstractValueCondition
	{
		public override bool IsMatching(Card card)
		{
			if (!card.Properties.TryGetValue(Field, out var val))
			{
				return true;
			}
			
			return val != Value;
		}
	}
}