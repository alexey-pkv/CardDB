using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Library.State
{
	public class StateManager : IStateManager
	{
		private ApplicationState m_currentState;
		private ManualResetEvent m_waiter = new(false);
		
		private IModule m_currentModule;
		private List<IModule> m_pendingModules = new();
		private Exception m_moduleError = null;
		
		
		private void OnComplete(IModule module, ApplicationState state, Exception e)
		{
			lock (this)
			{
				if (m_moduleError != null)
					return;
				
				if (m_currentState != state)
					return;
				
				if (!m_pendingModules.Remove(module))
					return;
				
				m_moduleError = e;
				
				if (e != null)
				{
					m_moduleError = e;
					Log.Info($"{m_currentState}: Module {module.Name} Failed! Aborting...", e);
					
					m_waiter.Set();
				}
				else
				{
					Log.Info($"{m_currentState}: Module {module.Name} done");
					
					if (m_pendingModules.Count == 0)
					{
						m_waiter.Set();
					}
				}
			}
		}
		
		
		public IModuleContainer Container { get; }
		
		
		public StateManager(IModuleContainer container, ApplicationState state)
		{
			Container = container;
			m_currentState = state;
		}
		
		
		public void SetModule(IModule module)
		{
			if (m_currentModule != null)
			{
				Log.Info($"{m_currentState}: Module {m_currentModule.Name} done");
			}
			
			m_currentModule = module;
			Log.Info($"{m_currentState}: Module {m_currentModule.Name}");
		}
		
		
		public IStateToken CreateToken()
		{
			if (m_currentModule == null)
				throw new Exception("Can not call CreateToken more then once");
			
			var module = m_currentModule;
			var state = m_currentState;
			
			lock (this)
			{
				m_pendingModules.Add(module);				
			}
			
			m_currentModule = null;
			
			return new StateToken(e =>
			{
				OnComplete(module, state, e);
			});
		}
		
		public void CompleteAfter(Action task)
		{
			CompleteAfter(() =>
			{
				task();
				return Task.CompletedTask;
			});
		}
		
		public void CompleteAfter(Task task)
		{
			CompleteAfter(() => task);
		}
		
		public void CompleteAfter(IEnumerable<Task> tasks)
		{
			CompleteAfter(() => Task.WhenAll(tasks));
		}
		
		public void CompleteAfter(Func<Task> task)
		{
			var action = m_currentState;
			var module = m_currentModule;
			var token = CreateToken();

			Task.Run(async () =>
			{
				try
				{
					await task();
					token.Complete();
				}
				catch (Exception e)
				{
					Log.Error($"Module {module.Name} did not complete successfully for {action}", e);
					token.Fail(e);
				}
			});
		}
		
		public void CompleteAfter<T>(Func<T, Task> task, T t)
		{
			CompleteAfter(() => task(t));
		}
		
		public void CompleteAfter<T1, T2>(Func<T1, T2, Task> task, T1 t1, T2 t2)
		{
			CompleteAfter(() => task(t1, t2));
		}
		
		public void Wait(float timeout)
		{
			if (m_currentModule != null)
			{
				Log.Info($"{m_currentState}: Module {m_currentModule.Name} done");
				m_currentModule = null;
			}
			
			lock (this)
			{
				if (m_pendingModules.Count == 0)
					return;
				
				m_waiter.Reset();
			}
			
			if (m_waiter.WaitOne((int)(timeout * 1000)))
			{
				if (m_moduleError != null)
					throw m_moduleError;
				
				return;
			}
			
			lock (this)
			{
				if (m_moduleError != null)
				{
					throw m_moduleError;
				}
				else if (m_pendingModules.Count != 0)
				{
					var modules = m_pendingModules.ToArray();
					
					m_pendingModules.Clear();
					
					throw new StateTimeoutException(modules, m_currentState);
				}
			}
		}
		
		public bool WaitSafe(float timeout)
		{
			try
			{
				Wait(timeout);
				return true;
			}
			catch (StateTimeoutException e)
			{
				Log.Error(e.Message, e);
				return false;
			}
		}
	}
}