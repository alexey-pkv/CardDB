using System.Threading;
using CardDB.Engine.Core;


namespace CardDB.Engine.Operators.ReIndex
{
	public class CountConsumer : IUpdatesConsumer
	{
		private int m_count = 0;
		
		public int Count => m_count;
		
		
		public void Consume(IUpdate update)
		{
			Interlocked.Increment(ref m_count);
		}
	}
}