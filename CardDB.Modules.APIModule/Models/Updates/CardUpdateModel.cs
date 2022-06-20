using System.Collections.Generic;
using CardDB.Updates;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class CardUpdateModel
	{
		public ulong sequence { get; }
		public UpdateTarget target => UpdateTarget.Card;
		
		public string cardId { get; }
		public UpdateType updateType { get; }
		public Dictionary<string, string> newProperties { get; }
		public Dictionary<string, string> previousProperties { get; }
		
		
		public CardUpdateModel(IUpdate update)
		{
			var cu = (CardUpdate)update;
			
			sequence = cu.Sequence;
			updateType = cu.UpdateType;
			cardId = cu.Card.ID;
			
			newProperties = cu.NewProperties;
			previousProperties = cu.PreviousProperties;
		}
	}
}