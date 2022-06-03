using System.Text.Json;
using CardDB.Conditions.ValueConditions;


namespace CardDB.Modules.APIModule.Input.Index.Types
{
	public class RegexConditionParser : IConditionTypeParser
	{
		public ICondition Parse(JsonElement source, string path)
		{
			return new RegexValueCondition
			{
				Field = IndexParserUtils.ParseStringField(source, "field", path),
				Regex = IndexParserUtils.ParseRegexField(source, "regex", path)
			};
		}
	}
}