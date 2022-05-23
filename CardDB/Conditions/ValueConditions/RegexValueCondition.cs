using System.Text.RegularExpressions;


namespace CardDB.Conditions.ValueConditions
{
	public class RegexValueCondition : AbstractValueCondition
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