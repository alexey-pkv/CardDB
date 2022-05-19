using System;
using System.Collections.Generic;

using CardDB.Engine.Core;


namespace CardDB.Engine.Operators.Actions
{
	public class ActionsQueue
	{
		private Queue<Action> m_actions = new();


		public IActionPersistence Persistence { get; set; }
		public ulong LastSequence { get; set; } = 0;
		public int Count => m_actions.Count;
		public bool IsEmpty => m_actions.Count == 0;
		
		
		public Action[] GetCurrentActionsList()
		{
			lock (m_actions)
			{
				return m_actions.ToArray();
			}
		}
		
		public void AddAction(Action action)
		{
			if (action.Sequence != 0)
				throw new InvalidOperationException("Only un-sequenced actions should be passed to AddAction");
			
			lock (m_actions)
			{
				LastSequence++;
				action.Sequence = LastSequence;
				
				m_actions.Enqueue(action);
			}
		}
		
		public void AddActions(IEnumerable<Action> actions)
		{
			lock (m_actions)
			{
				if (!IsEmpty)
					throw new InvalidOperationException("AddActions should be called only on an empty queue");

				foreach (var action in actions)
				{
					m_actions.Enqueue(action);
				}
			}
		}
		
		public Action Peek()
		{
			Action item;
			
			lock (m_actions)
			{
				m_actions.TryPeek(out item);
			}
			
			return item;
		}
		
		public Action Pop()
		{
			Action item;
			
			lock (m_actions)
			{
				m_actions.TryDequeue(out item);
			}
			
			return item;
		}
	}
}