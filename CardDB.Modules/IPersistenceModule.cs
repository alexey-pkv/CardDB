using System.Threading.Tasks;
using CardDB.Engine.Core;
using Library;


namespace CardDB.Modules
{
	public interface IPersistenceModule : IModule, IActionPersistence, IUpdatesConsumer
	{
		public Task<Bucket> Create(string name);
		public Task<Bucket> GetByName(string name);
	}
}