using System.Text.RegularExpressions;


namespace CardDB.Conditions.ValueConditions
{
	public class RegexValueCondition : AbstractFieldCondition
	{
		public Regex Regex { get; set; }
		
		
		public override bool IsMatching(Card card)
		{
			var regex = Regex;
			
			if (regex == null || !card.Properties.TryGetValue(Field, out var val))
			{
				return false;
			}
			
			return regex.IsMatch(val);
		}
	}
}