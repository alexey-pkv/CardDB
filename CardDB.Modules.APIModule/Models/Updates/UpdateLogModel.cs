using CardDB.Updates;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class UpdateLogModel
	{
		public string ID { get; set; }
		public CardUpdateModel CardUpdate { get; }
		public ViewUpdateModel ViewUpdate { get; }
		public IndexUpdateModel IndexUpdate { get; }
		
		
		public UpdateLogModel(UpdateLog updateLog)
		{
			ID = updateLog.RecordID;
			
			var update = updateLog.Update;
			
			if (update is CardUpdate) CardUpdate = new CardUpdateModel(update);
			else if (update is ViewUpdate) ViewUpdate = new ViewUpdateModel(update);
			else if (update is IndexUpdate) IndexUpdate = new IndexUpdateModel(update);
		}
	}
}