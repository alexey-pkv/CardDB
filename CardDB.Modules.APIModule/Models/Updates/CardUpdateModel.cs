using System.Collections.Generic;
using System.Text.Json.Serialization;
using CardDB.Updates;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class CardUpdateModel
	{
		public ulong sequence { get; }
		public UpdateTarget target => UpdateTarget.Card;
		
		public string cardId { get; }
		public UpdateType updateType { get; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, string> newProperties { get; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public Dictionary<string, string> previousProperties { get; }
		
		
		public CardUpdateModel(IUpdate update)
		{
			var cu = (CardUpdate)update;
			
			sequence = cu.Sequence;
			updateType = cu.UpdateType;
			cardId = cu.Card.ID;
			
			if (cu.NewProperties != null && cu.NewProperties.Count > 0)
				newProperties = cu.NewProperties;
			
			if (cu.PreviousProperties != null && cu.PreviousProperties.Count > 0)
				previousProperties = cu.PreviousProperties;
		}
	}
}