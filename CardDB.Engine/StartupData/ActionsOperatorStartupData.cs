using CardDB.Engine.Core;


namespace CardDB.Engine.StartupData
{
	public class ActionsOperatorStartupData
	{
		public DB DB { get; init; }
		public Action[] Actions { get; init; }
		public ulong LastSequenceID { get; init; }
		public IUpdatesConsumer UpdatesConsumer { get; init; }
		
		public bool HasActions => Actions != null && Actions.Length > 0;
	}
}