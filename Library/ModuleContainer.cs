using System;
using System.Collections;
using System.Collections.Generic;


namespace Library
{
	public class ModuleContainer : IModuleContainer
	{
		private List<IModule> m_modules = new();
		private Hashtable m_byType = new();
		private Hashtable m_byName = new();
		
		
		public T GetModule<T>() where T : class, IModule
		{
			var type = typeof(T);
			
			if (!m_byType.Contains(type))
				throw new Exception($"No module for type {type.FullName} defined!");
			
			return m_byType[typeof(T)] as T;
		}

		public void SetModule<T>(T t) where T : class, IModule
		{
			m_byName[t.Name] = t;
			m_byType[typeof(T)] = t;
			m_modules.Add(t);
		}

		public List<IModule> GetAll()
		{
			return m_modules;
		}

		public IModule GetByName(string name)
		{
			if (!m_byName.Contains(name))
				return null;
				
			return m_byName[name] as IModule;
		}

		public bool IsLoaded(string name)
		{
			return m_byName.Contains(name);
		}
	}
}