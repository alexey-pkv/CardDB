using System;
using Library.State;


namespace Library
{
	public class ModuleManager : IModuleManager
	{
		private float? m_timeout = null;
		private IConfig m_config;
		private IModuleContainer m_container;
		
		
		private float GetTimeout()
		{
			m_timeout ??= m_config.GetFloat("app.module_timeout", 30.0f);
			return (float)m_timeout;
		}
		
		private void UpdateAppState(ApplicationState state)
		{
			foreach (var module in m_container.GetAll())
			{
				module.AppStateChanged(state);
			}
		}
		
		private void SetupContainer()
		{
			foreach (var module in m_container.GetAll())
			{
				module.SetContainer(m_container);
			}
		}
		
		private void SetupConfig()
		{
			foreach (var module in m_container.GetAll())
			{
				module.SetConfig(m_config);
			}
		}
		
		private void ExecuteSequence(ApplicationState state, Action<IModule, StateManager> action)
		{
			var manager = new StateManager(m_container, state);
			
			Log.Info($"Switching to state {state}");
			UpdateAppState(state);

			foreach (var module in m_container.GetAll())
			{
				manager.SetModule(module);
				action(module, manager);
			}
			
			manager.Wait(GetTimeout());
		}

		#region Sequence
		
		private void Initializing()
		{
			ExecuteSequence(ApplicationState.Initializing, (module, manager) => module.Init());
		}
		
		private void PreStart()
		{
			ExecuteSequence(ApplicationState.PreStart, (module, manager) => module.PreLoad(manager));
		}

		private void Starting()
		{
			ExecuteSequence(ApplicationState.Starting, (module, manager) => module.Load(manager));
		}
		
		private void PreStop()
		{
			ExecuteSequence(ApplicationState.PreStop, (module, manager) => module.PreStop(manager));
		}
		
		private void Stopping()
		{
			ExecuteSequence(ApplicationState.Stopping, (module, manager) => module.Stop(manager));
		}

		private void Stopped()
		{
			ExecuteSequence(ApplicationState.Stopped, (module, manager) => module.Stopped(manager));
		}
		
		#endregion
		
		
		public ModuleManager()
		{
			m_container = new ModuleContainer();
		}
		
		
		public void Initialize(IConfig config)
		{
			m_config = config;
			
			SetupConfig();
			Initializing();
			SetupContainer();
		}
		
		public void Boot()
		{
			PreStart();
			Starting();
			
			UpdateAppState(ApplicationState.Running);
		}
		
		public void Stop()
		{
			PreStop();
			Stopping();
			Stopped();
		}
		
		public IModuleContainer Container()
		{
			return m_container;
		}
	}
}