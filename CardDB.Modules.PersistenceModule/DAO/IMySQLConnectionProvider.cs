using System.Collections.Generic;
using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base;
using MySql.Data.MySqlClient;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public interface IMySQLConnectionProvider
	{
		public Task<MySqlConnection> GetBare();
		public Task<MySqlConnection> Get();
		public Task<MySqlCommand> GetCommand();
		public Task<MySqlCommand> GetCommand(string command);
		public Task<MySqlCommand> GetCommand(string command, object[] bind);
		public Task<MySqlCommand> GetCommand(string command, IEnumerable<object> bind);

		public Task<object> ExecuteScalar(string command);
		public Task<object> ExecuteScalar(string command, object[] bind);
		public Task<object> ExecuteScalar(string command, IEnumerable<object> bind);
		
		public Task<long> Insert(string table, Dictionary<string, object> values);
		public Task<long?> InsertIgnore(string table, Dictionary<string, object> values);
		
		public Task<long> Insert<T>(string table, IDataModel<T> o);
		public Task<long?> InsertIgnore<T>(string table, IDataModel<T> o);
		
		public Task<long> Update(string table, Dictionary<string, object> values, Dictionary<string, object> byFields);
		public Task<long> Update<T>(string table, IDataModel<T> o);
		
		public Task<long> UpdateAll(string table, IEnumerable<Dictionary<string, object>> items, string[] byFields);
		public Task<long> UpdateAll<T>(string table, IEnumerable<IDataModel<T>> items);
		
		public Task<K> LoadOneByField<T, K>(string table, string field, string value) 
			where T : IDataModel<K>, new()
			where K : class;
		
		public Task<K[]> Select<T, K>(string select, object[] bind) 
			where T : IDataModel<K>, new()
			where K : class;
	}
}