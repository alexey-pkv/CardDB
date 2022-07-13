using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Library.Tasks
{
	public class SimpleTaskQueue
	{
		#region Data Members
		
		private Queue<Func<Task>> m_actions = new();
		private bool m_blIsRunning = false;
		
		#endregion
		
		#region Properties
		
		public int Count	=> m_actions.Count;
		
		#endregion
		
		#region Private Methods
		
		private async Task Execute()
		{
			Func<Task> a;
			
			lock (m_actions)
			{
				if (!m_actions.TryDequeue(out a))
				{
					m_blIsRunning = false;
					return;
				}
			}

			await a();
			
			#pragma warning disable 4014
			Task.Run(Execute);
			#pragma warning restore 4014
		}
		
		private async Task ExecuteWithTaskAwaiter(Func<Task> a, TaskCompletionSource source)
		{
			try
			{
				await a();
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
			Enqueue(() =>
			{
				a();
				return Task.CompletedTask;
			});
		}
		
		public void Enqueue(Func<Task> a)
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
		
		public void Enqueue(Func<Task> a, out Task awaitExecute)
		{
			awaitExecute = EnqueueAndWait(a);
		}
		
		public Task EnqueueAndWait(Action a)
		{
			return EnqueueAndWait(() =>
			{
				a();
				return Task.CompletedTask;
			});
		}
		
		public Task EnqueueAndWait(Func<Task> a)
		{
			TaskCompletionSource source = new();

			async Task WithAwaiter()
			{
				await ExecuteWithTaskAwaiter(a, source);
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