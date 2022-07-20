namespace CardDB.Updates
{
	public class ViewUpdate : IUpdate
	{
		public ulong Sequence { get; init; }
		public UpdateTarget TargetType => UpdateTarget.View;
		public UpdateType UpdateType { get; init; }
		
		
		public Card View { get; init; }
		
		
		public static ViewUpdate ViewCreated(Action a, Card view)
		{
			return new ViewUpdate
			{
				Sequence = a.Sequence,
				View = view,
				UpdateType = UpdateType.Added
			};
		}
		
		public static ViewUpdate ViewDeleted(Action a, Card view)
		{
			return new ViewUpdate
			{
				Sequence = a.Sequence,
				View = view,
				UpdateType = UpdateType.Removed
			};
		}
	}
}