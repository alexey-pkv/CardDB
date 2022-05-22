using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Library.Tasks
{
	public class SimpleTaskQueue
	{
		#region Data Members
		
		private Queue<Action> m_actions = new();
		private bool m_blIsRunning = false;
		
		#endregion
		
		#region Properties
		
		public int Count	=> m_actions.Count;
		
		#endregion
		
		#region Private Methods
		
		private void Execute()
		{
			Action a;
			
			lock (m_actions)
			{
				if (!m_actions.TryDequeue(out a))
				{
					m_blIsRunning = false;
					return;
				}
			}

			a();
			
			Task.Run(Execute);
		}
		
		private void ExecuteWithTaskAwaiter(Action a, TaskCompletionSource source)
		{
			try
			{
				a();
				source.SetResult();
			}
			catch (Exception e)
			{
				source.SetException(e);
			}
		}
		
		private void ScheduleTask()
		{
			if (!m_blIsRunning)
			{
				m_blIsRunning = true;
				Task.Run(Execute);
			}
		}
		
		#endregion
		
		#region Public Methods
		
		public void Enqueue(Action a)
		{
			lock (m_actions)
			{
				m_actions.Enqueue(a);
				ScheduleTask();
			}
		}
		
		public void Enqueue(Action a, out Task awaitExecute)
		{
			awaitExecute = EnqueueAndWait(a);
		}
		
		public Task EnqueueAndWait(Action a)
		{
			TaskCompletionSource source = new();

			void WithAwaiter()
			{
				ExecuteWithTaskAwaiter(a, source);
			}

			lock (m_actions)
			{
				m_actions.Enqueue(WithAwaiter);
				ScheduleTask();
			}
			
			return source.Task;
		}
		
		public Task WaitComplete()
		{
			TaskCompletionSource source = new();
			Enqueue(() => source.SetResult());
			return source.Task;
		}
		
		#endregion
	}
}