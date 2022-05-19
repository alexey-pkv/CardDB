using CardDB.Engine.Core;

namespace CardDB.Engine.Operators.Actions
{
	public class ActionsExecutor
	{
		private DB m_db;
		private IUpdatesConsumer m_consumer;
		
		
		public ActionsExecutor(DB db, IUpdatesConsumer consumer)
		{
			m_db = db;
			m_consumer = consumer;
		}
		
		
		public void Execute(Action a)
		{
			
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