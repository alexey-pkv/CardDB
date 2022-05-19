using System.Collections.Generic;

namespace CardDB.Updates
{
	public class CardUpdate : IUpdate
	{
		public long Sequence { get; set; }
		public UpdateTarget TargetType => UpdateTarget.Card;
		
		
		public Card Card { get; init; }
		public UpdateType UpdateType { get; init; }
		public Dictionary<string, string> NewProperties { get; init; }
		public Dictionary<string, string> PreviousProperties { get; init; }
		
		
		public static CardUpdate Deleted(Card card)
		{
			return new CardUpdate
			{
				Card = card,
				UpdateType = UpdateType.Removed
			};
		}
	}
}