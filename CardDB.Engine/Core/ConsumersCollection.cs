using System;
using System.Collections.Generic;

using Library;


namespace CardDB.Engine.Core
{
	public class ConsumersCollection : IUpdatesConsumer
	{
		private List<IUpdatesConsumer> m_consumers;
		
		
		public ConsumersCollection(IEnumerable<IUpdatesConsumer> consumers)
		{
			m_consumers = new List<IUpdatesConsumer>(consumers);
		}
		
		
		public void Consume(IUpdate update)
		{
			foreach (var consumer in m_consumers)
			{
				try
				{
					consumer.Consume(update);
				}
				catch (Exception e)
				{
					Log.Error($"Failed to consume payload ${update}", e);
				}
			}
		}
	}
}