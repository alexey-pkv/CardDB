using System;
using System.Linq;


namespace Library.State
{
	public class StateTimeoutException : CardDBException
	{
		private static string ListModuleNames(IModule[] pendingModules)
		{
			return String.Join(", " , pendingModules.Select(m => m.Name));
		}
		
		
		public StateTimeoutException(IModule[] pendingModules, ApplicationState state):
			base($"State update to {state} timed out for: {ListModuleNames(pendingModules)}")
		{}
	}
}