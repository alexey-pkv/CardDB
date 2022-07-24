using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base.DAO;
using CardDB.Modules.PersistenceModule.Models;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class BucketDAO : IBucketDAO
	{
		private const string TABLE	= "Bucket";
		
		
		private IMySQLConnectionProvider m_provider;
		
		
		public BucketDAO(IMySQLConnectionProvider connectionProvider)
		{
			m_provider = connectionProvider;
		}
		
		
		public async Task<bool> Save(Bucket bucket)
		{
			var model = new BucketModel(bucket);
			var id = await m_provider.InsertIgnore(TABLE, model);
			
			return id != null;
		}

		public async Task<Bucket> LoadByName(string name)
		{
			return await m_provider.LoadOneByField<BucketModel, Bucket>(TABLE, "Name", name);
		}

		public async Task<Bucket> LoadByID(string id)
		{
			return await m_provider.LoadOneByField<BucketModel, Bucket>(TABLE, "ID", id);
		}
	}
}