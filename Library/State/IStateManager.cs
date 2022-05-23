using System;
using System.Threading.Tasks;


namespace Library.State
{
	public interface IStateManager
	{
		public IModuleContainer Container { get; }
		public IStateToken CreateToken();
		public void CompleteAfter(Action task);
		public void CompleteAfter(Func<Task> task);
	}
}