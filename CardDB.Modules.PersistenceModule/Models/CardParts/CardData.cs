using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using CardDB.Indexing;
using Library;


namespace CardDB.Modules.PersistenceModule.Models.CardParts
{
	public class CardData
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, string> properties { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public StandardIndexerData index { get; set; }
		
		
		public CardData(Card c)
		{
			properties = new Dictionary<string, string>(c.Properties);
			
			if (c.IsView)
			{
				if (c.Indexer is not StandardIndexer sid)
					throw new UnauthorizedAccessException();
				
				index = new StandardIndexerData(sid);
			}
		}
		
		
		public static string GetData(Card c)
		{
			if (c.IsDeleted)
				return null;
			
			var cd = new CardData(c);
			return JSON.Serialize(cd);
		}
	}
}