namespace CardDB.Conditions.ValueConditions
{
	public class ContainsValueCondition : AbstractValueCondition
	{
		public override bool IsMatching(Card card)
		{
			if (!card.Properties.TryGetValue(Field, out var val))
			{
				return false;
			}
			
			return val.Contains(Value);
		}
	}
}