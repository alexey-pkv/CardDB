using System;
using Library.State;


namespace Library
{
	public abstract class AbstractModule : IModule
	{
		private IModuleContainer m_container;
		
		
		protected IModuleContainer Container => m_container;
		protected ApplicationState AppState { private set; get; }
		protected IConfig Config { private set; get; }
		
		
		protected bool IsStopping => AppState == ApplicationState.Stopping &&
		                             AppState == ApplicationState.Stopped;
		
		protected T GetModule<T>() where T: class, IModule
		{
			if (m_container == null)
				throw new InvalidOperationException($"{nameof(GetModule)} can be used only after the " +
					$"{nameof(SetContainer)} method was invoked");
			
			return m_container.GetModule<T>();
		}
		
		
		public abstract string Name { get; }
		
		public void SetConfig(IConfig config)
		{
			Config = config;
		}
		
		public void AppStateChanged(ApplicationState state)
		{
			AppState = state;
		}
		
		public virtual void SetContainer(IModuleContainer container)
		{
			m_container = container;
		}
		
		public virtual void Init() {}
		
		public virtual void PreLoad(IStateManager state) {}
		public virtual void Load(IStateManager state) {}
		public virtual void PreStop(IStateManager state) {}
		public virtual void Stop(IStateManager state) {}
		public virtual void Stopped(IStateManager state) {}
	}
}