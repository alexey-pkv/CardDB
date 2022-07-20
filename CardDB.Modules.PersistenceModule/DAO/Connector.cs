using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.Base;
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
		
		
		private MySqlParameter[] ToParams(object[] values)
		{
			return values
				.Select(v => new MySqlParameter { Value = v })
				.ToArray();
		}
		
		private MySqlParameter[] ToParams(IEnumerable<object> values)
		{
			return values
				.Select(v => new MySqlParameter { Value = v })
				.ToArray();
		}
		
		
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
			cmd.Parameters.AddRange(ToParams(bind));
			
			return cmd;
		}
		
		public Task<MySqlCommand> GetCommand(string command, IEnumerable<object> bind)
		{
			return GetCommand(command, bind.ToArray());
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
		
		public Task<object> ExecuteScalar(string command, IEnumerable<object> bind)
		{
			return ExecuteScalar(command, bind.ToArray());
		}
		
		
		public async Task<long> Insert(string table, Dictionary<string, object> values)
		{
			var columns = String.Join(", ", values.Keys);
			var placeholders =  String.Join(", ", Enumerable.Repeat("?", values.Count));
			
			var cmd = await GetCommand(
				$"INSERT INTO {table} " +
					$"({columns}) " +
				"VALUES " +
					$"({placeholders})",
				values.Values);
			
			await cmd.ExecuteNonQueryAsync();
			
			return await Task.FromResult(cmd.LastInsertedId);
		}
		
		
		public async Task<long> Insert<T>(string table, IDataModel<T> o)
		{
			long id;
			var values = o.ToData();
			
			if (o.IsAutoInc)
			{
				values.Remove(o.PrimaryID);
			}
			
			id = await Insert(table, values);
			
			if (o.IsAutoInc)
			{
				o.SetAutoIncID(id);
			}
			
			return id;
		}

		public Task<long> Update<T>(string table, IDataModel<T> o)
		{
			throw new NotImplementedException();
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
		
		
		public IActionDAO Action => new ActionDAO(this);
		public IItemDAO Item => new ItemDAO(this);
		
		#endregion
	}
}