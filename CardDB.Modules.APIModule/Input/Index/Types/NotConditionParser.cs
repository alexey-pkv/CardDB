using System.Text.Json;
using CardDB.Conditions;


namespace CardDB.Modules.APIModule.Input.Index.Types
{
	public class NotConditionParser : IConditionTypeParser
	{
		public ICondition Parse(JsonElement source, string path)
		{
			var child = IndexParserUtils.ParseConditionInField(source, "condition", path);
			
			if (child is NotCondition)
			{
				return ((NotCondition)child).Child;
			}
			
			return new NotCondition
			{
				Child = IndexParserUtils.ParseConditionInField(source, "condition", path)
			};
		}
	}
}