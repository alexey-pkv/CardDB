using log4net;
using log4net.Core;
using log4net.Layout;
using log4net.Appender;
using log4net.Repository.Hierarchy;


namespace Library
{
	public class LogRepository
	{
		private const string LOG_REPOSITORY = "CardDB";
		
		
		public static bool IsSetup { get; private set; }
		
		
		static LogRepository()
		{
			IsSetup = false;
		}
		
		
		private static PatternLayout GetLogPattern()
		{
			PatternLayout patternLayout = new PatternLayout
			{
				ConversionPattern = "%date [%logger] [%level]: %message%newline"
			};
			
			patternLayout.ActivateOptions();
			
			return patternLayout;
		}
		
		private static ConsoleAppender GetConsoleAppender()
		{
			var consoleAppender = new ConsoleAppender
			{
				Layout = GetLogPattern()
			};
			
			consoleAppender.ActivateOptions();
			
			return consoleAppender;
		}
		
		private static RollingFileAppender GetFileAppender(string log_path)
		{
			var fileAppender = new RollingFileAppender
			{
				AppendToFile = true,
				File = log_path + "/card_db.log",
				Layout = GetLogPattern(),
				MaxSizeRollBackups = 5,
				MaximumFileSize = "100MB",
				RollingStyle = RollingFileAppender.RollingMode.Size,
				StaticLogFileName = true
			};
			
			fileAppender.ActivateOptions();
			
			return fileAppender;
		}
		
		
		public static void Configure(
			bool logToConsole = false,
			Level level = null,
			string configPath = "")
		{
			IsSetup = true;
			
			if (level == null) level = Level.Warn;
			
			var hierarchy = (Hierarchy)LogManager.CreateRepository(LOG_REPOSITORY);
			
            hierarchy.Root.AddAppender(GetFileAppender(configPath));
            
            if (logToConsole)
            {
				hierarchy.Root.AddAppender(GetConsoleAppender());
			}
            
            hierarchy.Root.Level = (level == null ? Level.Warn : level);
            hierarchy.Configured = true;
		}
		
		public static ILog GetFor(string fileName = "")
		{
			var catFrom		= fileName.LastIndexOfAny(new []{'\\', '/'});
			var catTo		= fileName.LastIndexOf('.');
			var logName	= fileName.Substring(catFrom + 1, catTo - catFrom - 1);
			
			return LogManager.GetLogger(LOG_REPOSITORY, logName);
		}
		
		public static ILog Get([System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
		{
			return GetFor(fileName);
		}
	}
}