using System.Collections.Generic;

namespace CardDB.Modules.PersistenceModule.Base
{
	public interface IDataModel<T>
	{
		public bool IsAutoInc { get; }
		public string PrimaryID { get; }
		
		public void From(T obj);
		public void From(Dictionary<string, object> data);
		
		public Dictionary<string, object> ToData();
		
		public void SetAutoIncID(long val);
		public T GetObject();
	}
}