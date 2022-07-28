using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
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
		
		
		public async Task<long> Update(string table, Dictionary<string, object> values, Dictionary<string, object> byFields)
		{
			var updateColumns	= String.Join(" = ?, ", values.Keys) + " = ?";
			var whereColumns	= String.Join(" = ? AND ", byFields.Keys) + " = ?";
			var bind			= values.Values.Concat(byFields.Values).ToArray();
			
			var cmd = await GetCommand(
				$"UPDATE {table} " +
					$"SET {updateColumns} " + 
					$"WHERE {whereColumns}",
				bind);
			
			return await cmd.ExecuteNonQueryAsync();
		}

		public async Task<long> Update<T>(string table, IDataModel<T> o)
		{
			var data = o.ToUpdateData();
			var by = new Dictionary<string, object>();
			
			by[o.PrimaryID] = data[o.PrimaryID];
			data.Remove(o.PrimaryID);
			
			var res=  await Update(table, data, by);
			
			return res;
		}
		
		
		public async Task<long> UpdateAll(string table, IEnumerable<Dictionary<string, object>> items, string[] byFields)
		{
			List<object> binds = new List<object>();
			var insertString = new List<string>();
			var updateString = new List<string>();
			var fieldsString = new List<string>();

			foreach (var item in items)
			{
				if (fieldsString.Count == 0)
				{
					fieldsString.AddRange(item.Keys);
				}
				
				var bindArray = Enumerable.Repeat("?", item.Count).ToArray();
				
				insertString.Add($"({String.Join(", ", bindArray)})");
				binds.AddRange(item.Values);
			}
			
			foreach (var field in byFields)
			{
				updateString.Add($"{field} = VALUES(`{field}`)");
			}
			
			var cmd = await GetCommand(
				$"INSERT INTO {table} ({String.Join(", ", fieldsString)}) " +
					$"VALUES {String.Join(", ", insertString)} " + 
					$"ON DUPLICATE KEY UPDATE {String.Join(", ", updateString)}",
				binds.ToArray());
			
			return await cmd.ExecuteNonQueryAsync();
		}
		
		public async Task<long> UpdateAll<T>(string table, IEnumerable<IDataModel<T>> items)
		{
			List<Dictionary<string, object>> data = new List<Dictionary<string, object>>(items.Count());
			string[] byFields = null;
			
			foreach (var item in items)
			{
				var d = item.ToUpdateData();
				
				data.Add(new Dictionary<string, object>(d));
				
				if (byFields == null)
				{
					d.Remove(item.PrimaryID);
					byFields = d.Keys.ToArray();
				}
			}
			
			if (byFields == null)
				return 0;
			
			return await UpdateAll(table, data, byFields);
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