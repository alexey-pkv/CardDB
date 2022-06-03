using System.Text.Json;


namespace CardDB.Modules.APIModule.Input.Index
{
	public interface IConditionTypeParser
	{
		public ICondition Parse(JsonElement source, string path);
	}
}