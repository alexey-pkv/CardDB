using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface IBucketDAO
	{
		public Task<bool> Save(Bucket bucket);
		public Task<Bucket> LoadByName(string name);
		public Task<Bucket> LoadByID(string id);
	}
}