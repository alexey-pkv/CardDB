namespace CardDB.Conditions.FieldConditions
{
	public class FieldExistsCondition : AbstractFieldCondition
	{
		public override bool IsMatching(Card card)
		{
			return card.Properties.ContainsKey(Field);
		}
	}
}