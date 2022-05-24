using System.Text.Json.Serialization;


namespace CardDB.Modules.APIModule.Models
{
	public class AbstractWithCodeModel
	{
		[JsonIgnore]
		public int Code { get; init; }
	}
}