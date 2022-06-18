using System.Collections.Generic;
using CardDB.Engine.Core;
using CardDB.Modules.UpdatesLog;
using Library;


namespace CardDB.Modules
{
	public interface IUpdatesLogModule : IUpdatesConsumer, IModule
	{
		public IUpdateLog Get(string id);
		public IEnumerable<IUpdateLog> GetAfter(string id, int count);
		public IEnumerable<IUpdateLog> GetBetween(string id, string before, int count);
		public IEnumerable<IUpdateLog> Query(UpdatesLogQuery query);
	}
}