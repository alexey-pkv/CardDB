using System;
using System.Threading.Tasks;

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
		}
	}
}