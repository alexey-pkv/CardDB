using System.Threading.Tasks;
using System.Collections.Generic;

using CardDB.Updates;
using CardDB.Engine.Core;
using CardDB.Engine.StartupData;
using CardDB.Engine.Operators.ReIndex;


namespace CardDB.Engine.Operators
{
	public class ReIndexOperator : IUpdatesConsumer
	{
		#region Private Data Member
		
		private DB m_db;
		private IUpdatesConsumer m_consumer;
		
		private Queue<IUpdate> m_newViews = new();
		private Queue<IUpdate> m_cardUpdates = new();
		
		private bool m_blIsRunning;
		private Task m_viewUpdateTask;
		private Task m_cardUpdateTask;
		
		#endregion
		
		
		#region Private Methods
		
		private async Task ExecuteIndexer(ulong sequence, View view, Card card)
		{
			Indexer i = new()
			{
				Sequence		= sequence,
				DB				= m_db,
				UpdatesConsumer	= m_consumer,
				View			= view,
				Card			= card
			};
			
			await i.Index();
		}
		
		private async Task IndexCardsAction()
		{
			while (true)
			{
				IUpdate update;
				
				lock (m_cardUpdates)
				{
					if (!m_blIsRunning || !m_cardUpdates.TryDequeue(out update))
					{
						m_cardUpdateTask = null;
						return;
					}
				}
				
				await ExecuteIndexer(update.Sequence, null, ((CardUpdate)update).Card);
			}
		}

		private async Task IndexViewAction()
		{
			while (true)
			{
				IUpdate update;

				lock (m_newViews)
				{
					if (!m_blIsRunning || !m_cardUpdates.TryDequeue(out update))
					{
						m_viewUpdateTask = null;
						return;
					}
				}

				await ExecuteIndexer(update.Sequence, ((ViewUpdate)update).View, null);
			}
		}

		private void AddCardUpdate(IUpdate update)
		{
			lock (m_cardUpdates)
			{
				m_cardUpdates.Enqueue(update);
				
				if (m_cardUpdateTask != null)
					return;
				
				m_cardUpdateTask = Task.Run(IndexCardsAction);
			}
		}
		
		private void AddViewUpdate(IUpdate update)
		{
			if (update.UpdateType == UpdateType.Removed)
			{
				m_db.Views.RemoveView(((ViewUpdate)update).View.ID);
				return;
			}
			
			lock (m_newViews)
			{
				m_newViews.Enqueue(update);
				
				if (m_viewUpdateTask != null)
					return;
				
				m_viewUpdateTask = Task.Run(IndexViewAction);
			}
		}
		
		#endregion
		
		
		#region Methods
		
		public void Setup(ReIndexOperatorStartupData data)
		{
			m_db = data.DB;
			m_consumer = data.Consumer;
		}
		
		public void Consume(IUpdate update)
		{
			switch (update.TargetType)
			{
				case UpdateTarget.Card:
					AddCardUpdate(update);
					break;
				
				case UpdateTarget.View:
					AddViewUpdate(update);
					break;
				
				default:
					break;
			}
		}
		
		public async Task<IndexUpdate> Index(Card c, View v)
		{
			Indexer i = new()
			{
				Sequence		= c.SequenceID,
				DB				= m_db,
				UpdatesConsumer	= m_consumer
			};
			
			return await i.IndexDirect(v, c);
		}
		
		public void Start()
		{
			lock (m_cardUpdates)
			{
				if (m_blIsRunning)
					return;
				
				m_blIsRunning = true;
				
				if (m_cardUpdateTask == null && m_cardUpdates.Count > 1)
				{
					m_cardUpdateTask = IndexCardsAction();
				}
			}
			
			lock (m_newViews)
			{
				if (m_viewUpdateTask == null && m_newViews.Count > 1)
				{
					m_viewUpdateTask = IndexViewAction();
				}
			}
		}
		
		public async Task Stop()
		{
			lock (m_cardUpdates)
			{
				if (!m_blIsRunning)
					return;
			}
			
			while (
				m_cardUpdateTask != null || 
				m_viewUpdateTask != null)
			{
				await Task.Delay(1);
			}
		}
		
		#endregion
	}
}