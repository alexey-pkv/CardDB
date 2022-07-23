using System.Text.Json.Serialization;
using CardDB.Updates;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class UpdateLogModel
	{
		public string id { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public CardUpdateModel cardUpdate { get; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IndexUpdateModel indexUpdate { get; }
		
		
		public UpdateLogModel(UpdateLog updateLog)
		{
			id = updateLog.RecordID;
			
			var update = updateLog.Update;
			
			if (update is CardUpdate) cardUpdate = new CardUpdateModel(update);
			else if (update is IndexUpdate) indexUpdate = new IndexUpdateModel(update);
		}
	}
}