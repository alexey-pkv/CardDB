using System;
using System.Threading.Tasks;


namespace ConsoleApplication
{
	class Program
	{
		static int count = 10;
		
		public static async Task DoA()
		{
			await Task.Run(() => {});
		}
		
		public static async void Do()
		{
			var i = count--;
			
			await DoA();
			
			if (i > 0)
			{
				Console.WriteLine(i);
				Do();
				Console.WriteLine(i);
			}
		}
		
		
		static void Main(string[] args)
		{
			
		}
	}
}