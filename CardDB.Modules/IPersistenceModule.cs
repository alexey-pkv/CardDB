using System.Threading.Tasks;
using CardDB.Engine.Core;
using Library;


namespace CardDB.Modules
{
	public interface IPersistenceModule : IModule, IActionPersistence, IUpdatesConsumer
	{
		public Task<Bucket> CreateBucket(string name);
		public Task<Bucket> GetBucketByName(string name);
		public Task<Bucket> GetBucketByID(string id);
	}
}