using System;
using System.Threading.Tasks;

namespace Library
{
	public interface IApplication
	{
		public bool HandleError(Exception e) { return false; }
		public IConfig LoadConfig(string[] args) { return new Config(); }
		public void InitApp(IConfig config) {}
		public void ShutdownApp(IConfig config) {}
		public void SetupModules(IModuleContainer container, IConfig config);
		public Task Run(IModuleContainer container, IConfig config);
	}
}