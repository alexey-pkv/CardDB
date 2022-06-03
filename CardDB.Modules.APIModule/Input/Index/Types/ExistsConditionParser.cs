using System.Text.Json;
using CardDB.Conditions.FieldConditions;


namespace CardDB.Modules.APIModule.Input.Index.Types
{
	public class ExistsConditionParser : IConditionTypeParser
	{
		public ICondition Parse(JsonElement source, string path)
		{
			return new FieldExistsCondition
			{
				Field = IndexParserUtils.ParseStringField(source, "field", path)
			};
		}
	}
}