namespace CardDB.Engine.Core
{
	public interface IUpdatesConsumer
	{
		public void Consume(IUpdate update);
	}
}