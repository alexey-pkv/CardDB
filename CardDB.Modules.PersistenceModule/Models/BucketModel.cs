using System.Collections.Generic;
using CardDB.Modules.PersistenceModule.Base;


namespace CardDB.Modules.PersistenceModule.Models
{
	public class BucketModel : IDataModel<Bucket>
	{
		public bool IsAutoInc => false;
		public string PrimaryID => "ID";
		
		
		private Bucket m_bucket;
		
		
		public BucketModel() {}
		public BucketModel(Bucket b) { m_bucket = b; }
		
		
		public void From(Bucket obj)
		{
			m_bucket = obj;
		}

		public void From(Dictionary<string, object> data)
		{
			m_bucket = new Bucket();
			
			if (data.TryGetValue("ID", out var id) && id != null)
			{
				m_bucket.ID = (string)id;
			}
			
			if (data.TryGetValue("Name", out var name) && id != null)
			{
				m_bucket.Name = (string)name;
			}
		}

		public Dictionary<string, object> ToData()
		{
			return new Dictionary<string, object>
			{
				{ "ID",		m_bucket.ID },
				{ "Name",	m_bucket.Name },
			};
		}
		
		public Dictionary<string, object> ToUpdateData() => ToData();
		
		public Bucket GetObject()
		{
			return m_bucket;
		}

		public void SetAutoIncID(long val) {}
	}
}