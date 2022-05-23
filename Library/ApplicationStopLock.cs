using System.Threading;


namespace Library
{
	public static class ApplicationStopLock
	{
		private static volatile bool m_isStopping;
		private static ManualResetEvent m_event = new ManualResetEvent(false);



		public static void Stopping()
		{
			m_isStopping = true;
		}

		public static bool IsStopping()
		{
			return m_isStopping;
		}


		public static void Stopped()
		{
			m_event.Set();
		}

		public static void Wait()
		{
			m_event.WaitOne();
		}
	}
}