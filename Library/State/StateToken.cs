using System;


namespace Library.State
{
	public class StateToken : IStateToken
	{
		private bool m_isCalled;
		private Action m_callback;
		
		
		public StateToken(Action callback)
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
			
			m_callback();
		}
	}
}