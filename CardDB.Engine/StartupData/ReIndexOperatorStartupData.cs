using CardDB.Engine.Core;


namespace CardDB.Engine.StartupData
{
	public class ReIndexOperatorStartupData
	{
		public DB DB { get; init; }
		public IUpdatesConsumer Consumer { get; init; }
	}
}