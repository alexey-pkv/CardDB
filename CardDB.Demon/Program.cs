using System.Threading.Tasks;
using Library;


namespace CardDB.Demon
{
	class Program
	{
		public static async Task Main(string[] args)
		{
			await ApplicationManager.Run<CardDBApplication>(args);
			ApplicationStopLock.Stopped();
		}
	}
}