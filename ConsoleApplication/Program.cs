using System;
using System.Collections.Generic;

using CardDB;
using CardDB.Structs;
using Library.ID;

namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			for (var i = 0; i < 1000; i++)
			{
				Console.WriteLine(IDGenerator.Generate());
			}
		}
	}
}