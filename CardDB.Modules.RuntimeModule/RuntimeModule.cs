using Library;
using Library.State;
using System.Threading.Tasks;


namespace CardDB.Modules.RuntimeModule
{
	public class RuntimeModule : AbstractModule, IRuntimeModule
	{
		private TaskCompletionSource m_runSource = new();
		private bool m_blIsStopped;
		
		
		public override string Name => "Runtime";
		
		
		public Task Run()
		{
			return m_runSource.Task;
		}

		public void StopApp()
		{
			lock (this)
			{
				if (m_blIsStopped)
					return;
				
				m_blIsStopped = true;
			}
			
			m_runSource.SetResult();
		}


		public override void Load(IStateManager state)
		{
			GetModule<ISignalsModule>().OnStopSignal += StopApp;
		}

		public override void PreStop(IStateManager state)
		{
			lock (this)
			{
				if (!m_blIsStopped)
				{
					Log.Error("Application is stopping but no stop signal received by the module!");
				}
			}
		}
	}
}