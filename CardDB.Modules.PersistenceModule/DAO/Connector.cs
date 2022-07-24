using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

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
		
		public async Task<long?> InsertIgnore(string table, Dictionary<string, object> values)
		{
			var columns = String.Join(", ", values.Keys);
			var placeholders =  String.Join(", ", Enumerable.Repeat("?", values.Count));
			
			var cmd = await GetCommand(
				$"INSERT IGNORE INTO {table} " +
					$"({columns}) " +
				"VALUES " +
					$"({placeholders})",
				values.Values);
			
			var affected = await cmd.ExecuteNonQueryAsync();
			
			if (affected == 0)
			{
				return null;
			}
			else
			{
				return await Task.FromResult(cmd.LastInsertedId);
			}
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
		
		public async Task<long?> InsertIgnore<T>(string table, IDataModel<T> o)
		{
			long? id;
			var values = o.ToData();
			
			if (o.IsAutoInc)
			{
				values.Remove(o.PrimaryID);
			}
			
			id = await InsertIgnore(table, values);
			
			if (id != null && o.IsAutoInc)
			{
				o.SetAutoIncID((long)id);
			}
			
			return id;
		}

		public Task<long> Update<T>(string table, IDataModel<T> o)
		{
			throw new NotImplementedException();
		}
		
		public async Task<K> LoadOneByField<T, K>(string table, string field, string value)
			where T : IDataModel<K>, new()
			where K : class
		{
			var cmd = await GetCommand(
				$"SELECT * FROM {table} " +
					$"WHERE {field} = ? " +
					$"LIMIT 2",
				new object[]{ value });
			
			var res = await cmd.ExecuteReaderAsync();
			T model = default(T);

			while (await res.ReadAsync())
			{
				if (model != null)
					throw new CardDBException($"More than one row selected for {table} {field}={value}");
				
				var data = new Dictionary<string, object>();
				
				for (int i = 0; i < res.FieldCount; i++)
				{
					data[res.GetName(i)] = res.GetValue(i);
				}
				
				model = new T();
				model.From(data);
			}
			
			return model == null ? null : model.GetObject();
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
		public ICardDAO Card => new CardDAO(this);
		public IBucketDAO Bucket => new BucketDAO(this);
		
		#endregion
	}
}