using CardDB.Updates;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class ViewUpdateModel
	{
		public ulong sequence { get; }
		public UpdateTarget target => UpdateTarget.View;
		
		public string viewId { get; }
		public UpdateType updateType { get; }
		
		
		public ViewUpdateModel(IUpdate update)
		{
			var cu = (ViewUpdate)update;
			
			sequence = cu.Sequence;
			updateType = cu.UpdateType;
			viewId = cu.View.ID;
		}
	}
}