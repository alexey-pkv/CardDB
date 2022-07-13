using System.Text.Json;
using CardDB.Conditions.FieldConditions;


namespace CardDB.Modules.APIModule.Input.Index.Types
{
	public class IsValueConditionParser : IConditionTypeParser
	{
		public ICondition Parse(JsonElement source, string path)
		{
			return new IsValueCondition
			{
				Field = IndexParserUtils.ParseStringField(source, "field", path),
				Value = IndexParserUtils.ParseStringField(source, "value", path)
			};
		}
	}
}