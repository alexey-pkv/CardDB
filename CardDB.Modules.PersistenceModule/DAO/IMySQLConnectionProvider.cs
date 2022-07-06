using System.Threading.Tasks;
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

		public Task<object> ExecuteScalar(string command);
		public Task<object> ExecuteScalar(string command, object[] bind);
	}
}