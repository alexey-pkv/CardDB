using System;
using System.Collections.Generic;


namespace Library
{
	public interface IModuleContainer
	{
		public T GetModule<T>() where T: class, IModule; 
		public void AddModule<T>(T t) where T : class, IModule => SetModule(t);
		
		public void SetModule<T, V>()
			where V : class, T, new()
			where T : class, IModule
		{
			SetModule<T>(new V());
		}
		
		public void SetModule<T>(T t) where T : class, IModule;
		
		public List<IModule> GetAll();

		public IModule GetByName(string name);
		
		public bool IsLoaded(string name);
	}
	
	
	public static class Container
	{
		private static IModuleContainer m_container;
		
		
		public static T Init<T>(bool allowOverride = false) where T : IModuleContainer, new()
		{
			return (T)Init(new T(), allowOverride);
		}
		
		public static IModuleContainer Init(IModuleContainer container, bool allowOverride = false)
		{
			if (allowOverride)
				m_container = container;
			else if (m_container != null)
				throw new InvalidOperationException();
			else 
				m_container = container;
			
			return m_container;
		}
		
		
		public static T GetModule<T>() where T: class, IModule
		{
			return m_container.GetModule<T>();
		}
	}
}