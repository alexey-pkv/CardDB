using NClap;
using Library;
using System.Threading.Tasks;

using CardDB.Modules;
using CardDB.Modules.DBModule;
using CardDB.Modules.APIModule;
using CardDB.Modules.TestModule;
using CardDB.Modules.UpdatesLog;
using CardDB.Modules.RuntimeModule;
using CardDB.Modules.SignalsModule;

using log4net.Core;


namespace CardDB.Demon
{
	public class CardDBApplication : IApplication
	{
		private static CardDBCLIArguments ParseArguments(string[] args)
		{
			if (!CommandLineParser.TryParse(args, out CardDBCLIArguments arguments))
	        {
	            return null;
	        }
			
			if (arguments.ShowHelp)
			{
				System.Console.Write(CommandLineParser.GetUsageInfo(typeof(CardDBCLIArguments)));
				return null;
			}
			
			return arguments;
		}
		
		
		public IConfig LoadConfig(string[] args)
		{
			var arguments = ParseArguments(args);
			
			if (arguments == null)
				return null;
			
			Config config = new Config();
			
			config.Set("app.module_timeout",		"30.0");
			config.Set("app.config_dir",			arguments.ConfigDir);
			config.Set("app.log_dir",				arguments.LogDir);
			config.Set("app.log_to_console",		arguments.LogToConsole ? "1" : "");
			
			config.Set("server.host",				arguments.Host);
			config.Set("server.port",				arguments.Port.ToString());
			
			return config;
		}

		public void InitApp(IConfig config)
		{
			LogRepository.Configure(
				level: Level.All,
				logToConsole: true,
				configPath: config["app.log_dir"].Get()
			);
			
			Log.Info("[STATE] Starting");
		}

		public void SetupModules(IModuleContainer container, IConfig config)
		{
			container.SetModule<ITestModule>(new TestModule());
			container.SetModule<ISignalsModule>(new SignalsModule());
			container.SetModule<IRuntimeModule>(new RuntimeModule());
			container.SetModule<IDBModule>(new DBModule());
			container.SetModule<IAPIModule>(new APIModule());
			container.SetModule<IUpdatesLogModule>(new UpdatesLogModule());
		}

		public Task Run(IModuleContainer container, IConfig config)
		{
			return container.GetModule<IRuntimeModule>().Run();
		}
	}
}