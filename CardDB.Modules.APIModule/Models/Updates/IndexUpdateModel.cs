using CardDB.Updates;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class IndexUpdateModel
	{
		public ulong sequence { get; }
		public UpdateTarget target => UpdateTarget.Index;
		
		public string viewId { get; }
		public string cardId { get; }
		public UpdateType updateType { get; }
		public CardIndexModel lastIndex { get; }
		public CardIndexModel newIndex { get; }
		
		
		public IndexUpdateModel(IUpdate update)
		{
			var cu = (IndexUpdate)update;
			
			sequence = cu.Sequence;
			updateType = cu.UpdateType;
			viewId = cu.View.ID;
			cardId = cu.Card.ID;
			
			if (cu.LastIndex != null) lastIndex = new CardIndexModel(cu.LastIndex);
			if (cu.NewIndex != null) newIndex = new CardIndexModel(cu.NewIndex);
		}
	}
}