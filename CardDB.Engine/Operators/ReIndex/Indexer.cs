using CardDB.Updates;
using CardDB.Engine.Core;

using System.Threading.Tasks;


namespace CardDB.Engine.Operators.ReIndex
{
	public class Indexer
	{
		#region Setup
		
		public ulong Sequence { get; init; }
		public Card View { get; init; }
		public Card Card { get; init; }
		public DB DB { get; init; }
		public IUpdatesConsumer UpdatesConsumer { get; init; }
		
		#endregion
		
		
		#region Private Methods
		
		private async Task IndexCard()
		{
			var views = DB.Views.GetViews();
			
			foreach (var view in views)
			{
				if (view.IsDeleted)
					break;
				
				await IndexDirect(view, Card);
			}
		}
		
		private async Task IndexView()
		{
			var cards = DB.Cards.GetCards();
			
			foreach (var card in cards)
			{
				if (View.IsDeleted)
					break;
				
				await IndexDirect(View, card);
			}
		}
		
		private IndexUpdate IndexDirectSync(Card view, Card c)
		{
			IndexUpdate update;
			
			lock (c)
			{
				update = view.Index(Sequence, c);
			}
			
			if (update != null)
			{
				UpdatesConsumer?.Consume(update);
			}
			
			return update;
		}
		
		#endregion
		
		
		#region Public Methods
		
		public async Task<IndexUpdate> IndexDirect(Card view, Card card)
		{
			return await Task.Run(() => IndexDirectSync(view, card));
		}
		
		public async Task Index()
		{
			if (View != null)
			{
				await IndexView();
			}
			else if (Card != null)
			{
				await IndexCard();
			}
		}
		
		#endregion
	}
}