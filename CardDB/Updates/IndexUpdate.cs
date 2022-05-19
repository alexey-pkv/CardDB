namespace CardDB.Updates
{
	public class IndexUpdate : IUpdate
	{
		public long Sequence { get; set; }
		public UpdateTarget TargetType => UpdateTarget.Index;
		
		
		public View View { get; init; }
		public Card Card { get; init; }
		
		public UpdateType UpdateType { get; init; }
		public CardIndex LastIndex { get; init; } = null;
		public CardIndex NewIndex { get; init; } = null;
		
		
		public static IndexUpdate Removed(CardIndex index)
		{
			return new IndexUpdate
			{
				Card = index.Card,
				View = index.View,
				UpdateType = UpdateType.Removed,
				LastIndex = index
			};
		}
		
		public static IndexUpdate Added(CardIndex index)
		{
			return new IndexUpdate
			{
				Card = index.Card,
				View = index.View,
				UpdateType = UpdateType.Added,
				NewIndex = index
			};
		}
		
		public static IndexUpdate ReIndexed(CardIndex prev, CardIndex curr)
		{
			return new IndexUpdate
			{
				Card = curr.Card,
				View = curr.View,
				UpdateType = UpdateType.Modified,
				NewIndex = curr,
				LastIndex = prev
			};
		}
	}
}