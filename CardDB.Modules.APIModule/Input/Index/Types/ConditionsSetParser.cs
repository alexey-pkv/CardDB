using System.Text.Json;
using CardDB.Conditions;
using CardDB.Modules.APIModule.Input.Exceptions;


namespace CardDB.Modules.APIModule.Input.Index.Types
{
	public class ConditionsSetParser<T> : IConditionTypeParser where T : ConditionsSet, new()
	{
		public ICondition Parse(JsonElement source, string path)
		{
			var children = IndexParserUtils.ParseConditionsInField(source, "conditions", path);
			
			if (children.Count == 0)
				throw new InvalidConditionSetupException("At least one condition is required", path);
			else if (children.Count == 1)
				return children[0];
			
			return new T
			{
				Children = children
			};
		}
	}
}