using System.Threading.Tasks;
using Library.Tasks;

namespace CardDB.Engine.Core
{
	public class MemoryActionPersistence : IActionPersistence
	{
		private SimpleTaskQueue m_queue = new();
		
		private ulong m_sequence = 0;
		
		
		public Task Persist(Action a)
		{
			return m_queue.EnqueueAndWait(() =>
			{
				m_sequence++;
				a.Sequence = m_sequence;
			});
		}

		public Task Persist(Action a, System.Action<Action> callback)
		{
			return m_queue.EnqueueAndWait(() =>
			{
				m_sequence++;
				a.Sequence = m_sequence;
				callback(a);
			});
		}
	}
}