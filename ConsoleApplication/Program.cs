using System;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardDB;
using IniParser;
using Library;
using Library.ID;


namespace ConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			CardIndex.FromJSON("[null, \"123123812\"]");
		}
	}
}