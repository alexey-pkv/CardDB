namespace CardDB
{
	public class IndexAction
	{
		public View View { get; init; }
		public Card Card { get; init; }
		
		public ActionType ActionType { get; init; }
		public CardIndex LastIndex { get; init; } = null;
		public CardIndex NewIndex { get; init; } = null;
		
		
		public static IndexAction Removed(CardIndex index)
		{
			return new IndexAction
			{
				Card = index.Card,
				View = index.View,
				ActionType = ActionType.Removed,
				LastIndex = index
			};
		}
		
		public static IndexAction Added(CardIndex index)
		{
			return new IndexAction
			{
				Card = index.Card,
				View = index.View,
				ActionType = ActionType.Added,
				NewIndex = index
			};
		}
		
		public static IndexAction ReIndexed(CardIndex prev, CardIndex curr)
		{
			return new IndexAction
			{
				Card = curr.Card,
				View = curr.View,
				ActionType = ActionType.ReIndex,
				NewIndex = curr,
				LastIndex = prev
			};
		}
	}
}