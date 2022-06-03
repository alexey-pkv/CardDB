using System;
using System.Threading.Tasks;

using CardDB.Updates;
using CardDB.Engine.Core;

using Library;
using Library.ID;


namespace CardDB.Engine.Operators.Actions
{
	public class ActionsExecutor
	{
		private DB m_db;
		private IUpdatesConsumer m_consumer;
		
		
		private async Task ExecuteCreateCardAsync(Action a)
		{
			CardUpdate u = await a.CreateCard();
			
			if (u != null)
			{
				m_db.Cards.AddCard(u.Card);
				m_consumer.Consume(u);
			}
		}
		
		private async void ExecuteCreateView(Action a)
		{
			View v = new View(
				a.GeneratedID ?? await IDGenerator.Generate(),
				a.ViewIndex
			);
			
			m_db.Views.AddView(v);
			m_consumer.Consume(ViewUpdate.ViewCreated(a, v));
		}
		
		private void ExecuteDeleteView(Action a)
		{
			var view = m_db.Views.RemoveView(a.ViewID);
			
			if (view == null)
				return;
			
			lock (view)
			{
				view.IsDeleted = true;
			}
			
			m_consumer.Consume(ViewUpdate.ViewDeleted(a, view));
		}
		
		private async Task ExecuteCardsActionAsync(Action a)
		{
			foreach (var id in a.CardIDs)
			{
				if (!m_db.Cards.Cards.TryGetValue(id, out var card))
					continue;
				
				await Task.Run(() => Execute(a, card));
			}
		}
		
		
		public ActionsExecutor(DB db, IUpdatesConsumer consumer)
		{
			m_db = db;
			m_consumer = consumer;
		}
		
		
		public async Task ExecuteUnsafe(Action a)
		{
			switch (a.ActionType)
			{
				case ActionType.CreateCard:
					await ExecuteCreateCardAsync(a);
					break;
				
				case ActionType.DeleteCard:
				case ActionType.ModifyCard:
					await ExecuteCardsActionAsync(a);
					break;
				
				case ActionType.CreateView:
					ExecuteCreateView(a);
					break;
					
				case ActionType.DeleteView:
					ExecuteDeleteView(a);
					break;
				
				default:
					throw new InvalidOperationException($"Missing action for {a.ActionType}");
			}
		}
		
		public async Task Execute(Action a)
		{
			try
			{
				await ExecuteUnsafe(a);
			}
			catch (Exception e)
			{
				Log.Fatal($"Failed to execute action {a}!", e);
			}
		}
		
		public void Execute(Action a, Card c)
		{
			IUpdate update;
			
			lock (c)
			{
				if (c.SequenceID >= a.Sequence)
					return;
				
				update = a.UpdateCard(c);
			}
			
			if (update != null)
			{
				m_consumer.Consume(update);
			}
		}
	}
}