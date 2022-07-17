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
		public void CompleteAfter<T>(Func<T, Task> task, T param);
		public void CompleteAfter<T1, T2>(Func<T1, T2, Task> task, T1 param1, T2 param2);
	}
}