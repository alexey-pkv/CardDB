using System;
using System.Threading.Tasks;

using CardDB.API;
using CardDB.Engine;
using Library.ID;


namespace ConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			for (var i = 0; i < 100; i++)
			{
				Console.WriteLine(await IDGenerator.Generate());
			}
			
			return;
			
			var API = new APIApp();
			var engine = new DBEngine();
			
			engine.Start();
			
			await API.Run(new APIStartupData { Engine = engine }, args);
			await engine.Stop();
		}
	}
}