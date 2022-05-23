using Library;
using System.Threading.Tasks;


namespace CardDB.Modules
{
	public interface IRuntimeModule : IModule
	{
		public Task Run();
		public void StopApp();
	}
}