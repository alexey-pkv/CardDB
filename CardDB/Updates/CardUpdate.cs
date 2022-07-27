using System.Collections.Generic;


namespace CardDB.Updates
{
	public class CardUpdate : IUpdate
	{
		public Bucket Bucket { get; init; }
		public ulong Sequence { get; init; }
		public UpdateTarget TargetType => UpdateTarget.Card;
		
		
		public Card Card { get; init; }
		public string CardID => Card.ID;
		public UpdateType UpdateType { get; init; }
		public Dictionary<string, string> NewProperties { get; init; }
		public Dictionary<string, string> PreviousProperties { get; init; }
		
		
		public static CardUpdate Deleted(Action a, Card card)
		{
			return new CardUpdate
			{
				Sequence = a.Sequence,
				Card = card,
				UpdateType = UpdateType.Removed
			};
		}
		
		public static CardUpdate CardCreated(Action a, Card card)
		{
			return new CardUpdate
			{
				Sequence = a.Sequence,
				Card = card,
				NewProperties = new Dictionary<string, string>(card.Properties),
				UpdateType = UpdateType.Added
			};
		}
	}
}