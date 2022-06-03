using System.Text.Json;
using CardDB.Modules.APIModule.Input.Index;


namespace CardDB.Modules.APIModule.Input
{
	public static class IndexParser
	{
		public static ICondition Parse(JsonElement element)
		{
			return IndexParserUtils.ParseRecursive(element, "<ROOT>");
		}
	}
}