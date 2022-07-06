using System;
using System.Threading.Tasks;
using CardDB.MySQL;
using Library.ID;
using MySql.Data.MySqlClient;


namespace ConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var a = await DatabaseSetup.GetCreateDB();
			
			
			Console.WriteLine(await IDGenerator.Generate());
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