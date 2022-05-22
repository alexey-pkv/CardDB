using System.Threading.Tasks;


namespace CardDB.Engine.Core
{
	public interface IActionPersistence
	{
		public Task Persist(Action a);
		public Task Persist(Action a, System.Action<Action> callback);
	}
}