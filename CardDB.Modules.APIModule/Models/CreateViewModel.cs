using System.Text.Json;


namespace CardDB.Modules.APIModule.Models
{
	public class CreateViewModel
	{
		public JsonElement index { get; set; }
		public string[] order { get; set; }
	}
}