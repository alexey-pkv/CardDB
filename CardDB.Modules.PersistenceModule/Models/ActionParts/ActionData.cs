using System.Collections.Generic;
using System.Text.Json.Serialization;

using Library;


namespace CardDB.Modules.PersistenceModule.Models.ActionParts
{
	public class ActionData
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, string> properties { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public HashSet<string> deleted_properties { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public HashSet<string> card_ids { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string view_id { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public IndexerData view_index { get; set; }
		
		
		public ActionData() {}
		public ActionData(Action a)
		{
			properties = a.Properties;
			deleted_properties = a.DeletedProperties;
			card_ids = a.CardIDs;
			view_id = a.ViewID;
			
			if (a.ViewIndex != null)
			{
				view_index = new IndexerData(a.ViewIndex);
			}
		}
		
		public void Setup(Action a)
		{
			a.Properties = properties;
			a.DeletedProperties = deleted_properties;
			a.CardIDs = card_ids;
			a.ViewID = view_id;
			
			if (view_index != null)
			{
				view_index.Setup(a);
			}
		}
		
		
		public static string GetData(Action a)
		{
			var ad = new ActionData(a);
			return JSON.Serialize(ad);
		}
		
		public static void SetData(Action a, string data)
		{
			var ad = JSON.Deserialize<ActionData>(data);
			ad.Setup(a);
		}
	}
}