using Library;

using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace CardDB.Modules.PersistenceModule.Models.ItemParts
{
	public class CardData
	{
		public string id { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, string> properties { get; set; }
		
		
		public CardData(Card c)
		{
			id = c.ID;
			properties = new Dictionary<string, string>(c.Properties);
		}
		
		
		public static string GetData(Card c)
		{
			var cd = new CardData(c);
			return JSON.Serialize(cd);
		}
	}
}