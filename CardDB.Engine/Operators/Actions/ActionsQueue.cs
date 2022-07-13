using System;
using System.Collections.Generic;

using CardDB.Engine.Core;


namespace CardDB.Engine.Operators.Actions
{
	public class ActionsQueue
	{
		private Queue<Action> m_actions = new();
		
		
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
			if (action.Sequence == 0)
				throw new InvalidOperationException("Only sequenced actions should be passed to AddAction");
			else if (LastSequence == 0)
				LastSequence = action.Sequence - 1;
			else if (action.Sequence != LastSequence + 1)
				throw new InvalidOperationException($"Action out of sequence! Waiting for " +
					$"{LastSequence + 1} but got {action.Sequence}");
		
			LastSequence = action.Sequence;
			
			lock (m_actions)
			{
				m_actions.Enqueue(action);
			}
		}
		
		public void AddActions(IEnumerable<Action> actions)
		{
			foreach (var action in actions)
			{
				AddAction(action);
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