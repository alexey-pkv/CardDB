namespace CardDB.Conditions.FieldConditions
{
	public class FieldNotExistsCondition : AbstractFieldCondition
	{
		public override bool IsMatching(Card card)
		{
			return !card.Properties.ContainsKey(Field);
		}
	}
}