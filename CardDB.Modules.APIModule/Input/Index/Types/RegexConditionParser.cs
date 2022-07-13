using System.Text.Json;
using CardDB.Conditions.FieldConditions;


namespace CardDB.Modules.APIModule.Input.Index.Types
{
	public class RegexConditionParser : IConditionTypeParser
	{
		public ICondition Parse(JsonElement source, string path)
		{
			return new RegexCondition
			{
				Field = IndexParserUtils.ParseStringField(source, "field", path),
				Regex = IndexParserUtils.ParseRegexField(source, "regex", path)
			};
		}
	}
}