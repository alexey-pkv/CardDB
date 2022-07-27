namespace CardDB.Updates
{
	public class IndexUpdate : IUpdate
	{
		public Bucket Bucket { get; init; }
		public ulong Sequence { get; init; }
		public UpdateTarget TargetType => UpdateTarget.Index;
		
		
		public Card Card { get; init; }
		public Card View { get; init; }
		
		public UpdateType UpdateType { get; init; }
		public CardIndex LastIndex { get; init; } = null;
		public CardIndex NewIndex { get; init; } = null;
		
		
		public static IndexUpdate Removed(ulong sequence, CardIndex index)
		{
			return new IndexUpdate
			{
				Sequence = sequence,
				Card = index.Card,
				View = index.View,
				UpdateType = UpdateType.Removed,
				LastIndex = index
			};
		}
		
		public static IndexUpdate Added(ulong sequence, CardIndex index)
		{
			return new IndexUpdate
			{
				Sequence = sequence,
				Card = index.Card,
				View = index.View,
				UpdateType = UpdateType.Added,
				NewIndex = index
			};
		}
		
		public static IndexUpdate ReIndexed(ulong sequence, CardIndex prev, CardIndex curr)
		{
			return new IndexUpdate
			{
				Sequence = sequence,
				Card = curr.Card,
				View = curr.View,
				UpdateType = UpdateType.Modified,
				NewIndex = curr,
				LastIndex = prev
			};
		}
	}
}