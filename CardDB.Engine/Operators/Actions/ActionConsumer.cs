using System;
using System.Threading.Tasks;


namespace CardDB.Engine.Operators.Actions
{
	public class ActionConsumer
	{
		private Func<Task<bool>> m_callback;
		
		private TaskCompletionSource m_waitStop = null;
		
		private bool m_blRun		= false;
		private bool m_blIsTainted	= false;
		private bool m_blIsRunning	= false;
		
		
		private void ScheduleIfNotRunning()
		{
			if (!m_blRun || m_blIsRunning)
				return;
			
			m_blIsRunning = true;
			
			Execute();
		}
		
		private bool IsStopping()
		{
			if (m_blRun)
				return false;
			
			m_blIsRunning = false;
			m_waitStop?.SetResult();
			return true;
		}
		
		private async void Execute()
		{
			lock (this)
			{
				if (IsStopping())
					return;
				
				m_blIsTainted = false;
			}
			
			var result = await m_callback();
			
			lock (this)
			{
				if (IsStopping())
					return;
				
				if (result)
				{
					m_blIsTainted = true;
				}
				
				if (m_blIsTainted)
				{
					m_blIsTainted = true;
					Execute();
				}
				else
				{
					m_blIsRunning = false;
				}
			}
		}
		
		
		public ActionConsumer(Func<Task<bool>> callback)
		{
			m_callback = callback;
		}
		
		public void Tainted()
		{
			lock (this)
			{
				if (m_blIsTainted)
					return;
				
				m_blIsTainted = true;
				
				ScheduleIfNotRunning();
			}
		}
		
		public void Start()
		{
			lock (this)
			{
				if (m_blRun)
					return;
				
				m_blRun = true;
				
				if (m_blIsTainted)
				{
					ScheduleIfNotRunning();
				}
			}
		}
		
		public Task Stop()
		{
			lock (this)
			{
				m_blRun = false;
				
				if (m_blIsRunning)
				{
					m_waitStop = new TaskCompletionSource();
					return m_waitStop.Task;
				}
				else
				{
					return Task.CompletedTask;
				}
			}
		}
	}
}