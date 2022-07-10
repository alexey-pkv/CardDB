using System.Collections.Generic;

namespace CardDB.Modules.PersistenceModule.Base
{
	public interface IDataModel<T>
	{
		public string PrimaryID { get; }
		
		public void From(T obj);
		public void From(Dictionary<string, object> data);
		
		public Dictionary<string, object> ToData();
		public T ToObject(); 
		
		public void SetAutoIncID(long val);
	}
}