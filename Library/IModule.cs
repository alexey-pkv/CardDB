using System;
using Library.State;


namespace Library
{
	public interface IModule
	{
		public String Name { get; }
		
		public void AppStateChanged(ApplicationState state);
		public void SetConfig(IConfig config);
		
		public void Init();
		public void SetContainer(IModuleContainer container);
		
		public void PreLoad(IStateManager state);
		public void Load(IStateManager state);
		public void PreStop(IStateManager state);
		public void Stop(IStateManager state);
		public void Stopped(IStateManager state);
	}
}