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
		public Task<long> Update<T>(string table, IDataModel<T> o);
		
		public Task<K> LoadOneByField<T, K>(string table, string field, string value) 
			where T : IDataModel<K>, new()
			where K : class;
	}
}