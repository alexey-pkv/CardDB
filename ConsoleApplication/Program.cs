using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CardDB;
using CardDB.Structs;
using Library.ID;

namespace ConsoleApplication
{
	class Program
	{
		public static async void Do()
		{
			Thread.Sleep(100);
			// await Task.Delay(100);
			Console.WriteLine(2);
		}
		
		
		static void Main(string[] args)
		{
			Console.WriteLine(1);
			Do();
			Console.WriteLine(3);
			Thread.Sleep(200);
		}
	}
}