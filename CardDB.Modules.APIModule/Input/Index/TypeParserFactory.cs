using System.Text.Json;
using CardDB.Conditions;
using CardDB.Modules.APIModule.Input.Exceptions;
using CardDB.Modules.APIModule.Input.Index.Types;

namespace CardDB.Modules.APIModule.Input.Index
{
	public static class TypeParserFactory
	{
		public static IConditionTypeParser GetType(JsonElement element, string path)
		{
			IndexParserUtils.ValidateObject(element, path);
			
			if (!element.TryGetProperty("type", out var type))
			{
				throw new InvalidConditionSetupException("Missing property `type`", path);
			}
			else if (type.ValueKind != JsonValueKind.String)
			{
				throw new InvalidConditionSetupException(
					$"`type` must be string, but got {type.ValueKind} instead", path);
			}
			
			var typeValue = type.GetString();

			switch (typeValue.ToLower())
			{
				case "and":
					return new ConditionsSetParser<AndCondition>();
				case "or":
					return new ConditionsSetParser<OrCondition>();
				case "not":
					return new NotConditionParser();
				case "exists":
					return new ExistsConditionParser();
				
				default:
					throw new InvalidConditionSetupException($"Unexpected type {typeValue}", path);
			}
		}
	}
}