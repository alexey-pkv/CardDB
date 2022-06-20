using System.Collections.Generic;
using CardDB.Engine.Core;
using CardDB.Modules.UpdatesLog;
using Library;


namespace CardDB.Modules
{
	public interface IUpdatesLogModule : IUpdatesConsumer, IModule
	{
		public UpdateLog Get(string id);
		public IEnumerable<UpdateLog> GetAfter(string id, int count);
		public IEnumerable<UpdateLog> GetBetween(string id, string before, int count);
		public IEnumerable<UpdateLog> Query(UpdatesLogQuery query);
	}
}