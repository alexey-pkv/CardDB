using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base.DAO;
using Library;
using MySql.Data.MySqlClient;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class Connector : IConnector, IMySQLConnectionProvider
	{
		private string m_server = "localhost";
		private string m_user = "root";
		private string m_password = "";
		private string m_db = "cards"; 
		
		
		private string ConnectionString => $"Server={m_server}; User={m_user}; password={m_password}";
		
		
		public Connector(IConfig config)
		{
			// TODO: Load config
		}
		
		public async Task<MySqlConnection> GetBare()
		{
			var conn = new MySqlConnection(ConnectionString);
			
			await conn.OpenAsync();
			
			return conn;
		}
		
		public async Task<MySqlConnection> Get()
		{
			var conn = await GetBare();
			
			var cmd = conn.CreateCommand();
			cmd.CommandText = $"USE {m_db}";
			await cmd.ExecuteNonQueryAsync();
			
			return conn;
		}

		public async Task<MySqlCommand> GetCommand()
		{
			var conn = await Get();
			return conn.CreateCommand();
		}
		
		public async Task<MySqlCommand> GetCommand(string command)
		{
			var cmd = await GetCommand();
			
			cmd.CommandText = command;
			
			return cmd;
		}
		
		public async Task<MySqlCommand> GetCommand(string command, object[] bind)
		{
			var cmd = await GetCommand();
			
			cmd.CommandText = command;
			cmd.Parameters.AddRange(bind);
			
			return cmd;
		}
		
		public async Task<object> ExecuteScalar(string command)
		{
			var cmd = await GetCommand(command);
			return await cmd.ExecuteScalarAsync();
		}
		
		public async Task<object> ExecuteScalar(string command, object[] bind)
		{
			var cmd = await GetCommand(command, bind);
			return await cmd.ExecuteScalarAsync();
		}
		
		
		#region Connector
		
		public async Task Test()
		{
			var conn = new MySqlConnection(ConnectionString);
			
			await conn.OpenAsync();
			
			var cmd = conn.CreateCommand();
			
			cmd.CommandText = "SELECT 1";
			await cmd.ExecuteScalarAsync();
		}

		public async Task CreateDB()
		{
			throw new System.NotImplementedException();
		}
		
		
		public IActionDAO Action => new ActionDAO(this);
		
		#endregion
	}
}