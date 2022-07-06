using System;


namespace Library.State
{
	public class StateToken : IStateToken
	{
		private bool m_isCalled;
		private Action<Exception> m_callback;
		
		
		public StateToken(Action<Exception> callback)
		{
			m_isCalled = false;
			m_callback = callback;
		}
		
		
		public void Complete()
		{
			lock (this)
			{
				if (m_isCalled) 
					return;
				
				m_isCalled = true;
			}
			
			m_callback(null);
		}
		
		public void Fail(Exception e)
		{
			lock (this)
			{
				if (m_isCalled) 
					return;
				
				m_isCalled = true;
			}
			
			m_callback(e);
		}
	}
}