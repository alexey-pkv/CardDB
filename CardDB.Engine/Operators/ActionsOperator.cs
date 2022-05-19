using System.Threading.Tasks;

using CardDB.Engine.StartupData;
using CardDB.Engine.Operators.Actions;


namespace CardDB.Engine.Operators
{
	public class ActionsOperator : IActionsOperator
	{
		#region Private Data Members
		
		private ActionsExecutor	m_executor;
		private ActionConsumer	m_consumer;
		private ActionsQueue	m_queue = new();
		
		#endregion
		
		#region Properties
		
		public int QueueSize => m_queue.Count;
		
		#endregion
		
		#region Private Methods
		
		private async Task<bool> ExecuteNextAction()
		{
			var action = m_queue.Peek();
			
			if (action != null)
			{
				await m_executor.Execute(action);
				m_queue.Pop();
			}
			
			return !m_queue.IsEmpty;
		}
		
		#endregion
		
		#region Constructor
		
		public ActionsOperator()
		{
			m_consumer = new ActionConsumer(ExecuteNextAction);
		}
		
		#endregion
		
		#region Methods

		public void Setup(ActionsOperatorStartupData data)
		{
			m_executor = new ActionsExecutor(data.DB, data.UpdatesConsumer);
			m_queue.LastSequence = data.LastSequenceID;
			
			if (data.HasActions)
			{
				m_queue.AddActions(data.Actions);
			}
		}
		
		public void AddAction(Action action)
		{
			m_consumer.Tainted();
		}
		
		public void ForceUpdateCard(Card c)
		{
			var actions = m_queue.GetCurrentActionsList();
			
			foreach (var action in actions)
			{
				m_executor.Execute(action, c);
			}
		}
		
		public void StartConsumer()
		{
			m_consumer.Start();
		}
		
		public Task StopConsumer()
		{
			return m_consumer.Stop();
		}
		
		#endregion
	}
}