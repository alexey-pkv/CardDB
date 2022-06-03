using System.Collections.Generic;


namespace CardDB.Modules.APIModule.Models
{
	public class CardModel
	{
		public string id { get; }
		public Dictionary<string, string> properties { get; }
		
		
		public CardModel(Card c)
		{
			id = c.ID;
			
			lock (c)
			{
				properties = new Dictionary<string, string>(c.Properties);
			}
		}
	}
}