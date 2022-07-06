using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;

namespace CardDB.MySQL
{
	public static class DatabaseSetup
	{
		private const string CREATE_DB_NAME	= "mysql_create_db.sql";
		
		
		public static async Task<string[]> GetCreateDB()
		{
			String content;
			var ass = System.Reflection.Assembly.GetExecutingAssembly();
			var name = ass
				.GetManifestResourceNames()
				.First(s => s.EndsWith(CREATE_DB_NAME));
			
			if (name == null)
			{
				throw new Exception("Create db not found!!!");
			}

			await using var stream = ass.GetManifestResourceStream(name);
			
			if (stream == null)
			{
				throw new Exception("Failed to open stream!!!");
			}

			using var reader = new StreamReader(stream);
			
			content = await reader.ReadToEndAsync();
			
			return content
				.Split(';')
				.Select(s => s.Trim())
				.Where(s => s.Length > 0)
				.ToArray();
		}
	}
}