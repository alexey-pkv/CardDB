using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardDB.Modules.PersistenceModule.DAO;
using CardDB.MySQL;
using Library;
using Library.ID;
using MySql.Data.MySqlClient;


namespace ConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var a = new Connector(new Config());
			
			
			
			var indt = await a.Insert("Server", new Dictionary<string, object>
			{
				{ "SequenceID", 123 }
			});
			
			Console.WriteLine(indt);
			
			return;
			
			var connStr = "Server=localhost; Database=oktopost; User=root; password=";
			var conn = new MySqlConnection(connStr);
			
			await conn.OpenAsync();
			
			var cmd = new MySqlCommand();
			
			// cmd.CommandText
			cmd.Connection = conn;
			
			var res = await cmd.ExecuteScalarAsync();
			
			Console.WriteLine($"Got: {res}");
		}
	}
}