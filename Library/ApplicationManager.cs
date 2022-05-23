using System;
using System.Threading.Tasks;


namespace Library
{
	public static class ApplicationManager
	{
		private static async Task RunUnsafe(IApplication applicationConfig, string[] args)
		{
			var manager = new ModuleManager();
			var container = Container.Init(manager.Container());
			var config = applicationConfig.LoadConfig(args);
			
			if (config == null)
			{
				return;
			}
			
			applicationConfig.SetupModules(container, config);
			applicationConfig.InitApp(config);
			
			manager.Initialize(config);
			manager.Boot();
			await applicationConfig.Run(container, config);
			manager.Stop();
			
			applicationConfig.ShutdownApp(config);
		}
		
		
		public static async Task Run<T>(string[] args) where T : IApplication, new()
		{
			await Run(new T(), args);
		}
		
		public static async Task Run(IApplication applicationConfig, string[] args)
		{
			try
			{
				await RunUnsafe(applicationConfig, args);
			}
			catch (Exception e)
			{
				if (applicationConfig.HandleError(e))
				{
					return;
				}
				else if (LogRepository.IsSetup)
				{
					Log.Fatal("An error was thrown in the main thread. Aborting...", e);
				}
				else
				{
					Console.WriteLine(e);
				}
			}
		}
	}
}