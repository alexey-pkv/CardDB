using CardDB.Engine.Core;
using Library;


namespace CardDB.Engine
{
	public class LogConsumer : IUpdatesConsumer
	{
		private IUpdatesConsumer m_next;
		
		
		public LogConsumer(IUpdatesConsumer next)
		{
			m_next = next;
		}
		
		
		public void Consume(IUpdate update)
		{
			Log.Info($"[UPDATE] Got: {update.UpdateType} - {update.TargetType}");
			m_next?.Consume(update);
		}
	}
}