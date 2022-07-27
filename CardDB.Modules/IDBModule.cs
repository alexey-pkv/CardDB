using System.Threading.Tasks;
using CardDB.Engine;
using Library;


namespace CardDB.Modules
{
	public interface IDBModule : IModule
	{
		public DBEngine GetEngine(Bucket bucket);
		public DBEngine GetOrCreateEngine(Bucket bucket);
		public DBEngine CreateEngine(Bucket bucket);
		
		public DB GetDB(Bucket bucket);
		public DB GetOrCreateDB(Bucket bucket);
		public DB CreateDB(Bucket bucket);
		
		public bool TryGetView(Bucket b, string id, out Card view);
		
		public Task AddAction(Bucket b, Action action);
	}
}