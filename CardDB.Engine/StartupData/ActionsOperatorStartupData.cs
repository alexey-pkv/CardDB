using CardDB.Engine.Core;


namespace CardDB.Engine.StartupData
{
	public class ActionsOperatorStartupData
	{
		public DB DB { get; set; }
		public Action[] Actions { get; set; } = null;
		public ulong LastSequenceID { get; set; } = 0;
		
		public IUpdatesConsumer UpdatesConsumer { get; set; }
		public IActionPersistence Persistence { get; set; }
		
		
		public bool HasActions => Actions != null && Actions.Length > 0;
	}
}