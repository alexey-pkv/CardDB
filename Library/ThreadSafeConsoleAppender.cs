using System;
using log4net.Core;
using log4net.Appender;


namespace Library
{
	public class ThreadSafeConsoleAppender : ConsoleAppender
	{
		private Object m_lock = new object();
		
		
		protected override void Append(LoggingEvent loggingEvent)
		{
			lock (m_lock)
			{
				base.Append(loggingEvent);
			}
		}
	}
}