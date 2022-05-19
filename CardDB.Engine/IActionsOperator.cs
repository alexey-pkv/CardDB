using System.Threading.Tasks;
using CardDB.Engine.StartupData;


namespace CardDB.Engine
{
	public interface IActionsOperator
	{
		public void Setup(ActionsOperatorStartupData data);
		
		public void AddAction(Action action);
		public void ForceUpdateCard(Card c);
		
		public void StartConsumer();
		public Task StopConsumer();
	}
}