using System;
using System.Linq;
using System.Collections.Generic;

using Library;


namespace CardDB.Modules.UpdatesLog
{
	public class UpdatesLogModule : AbstractModule, IUpdatesLogModule
	{
		public override string Name => "UpdatesLog";
		
		
		public void Consume(IUpdate update)
		{
			throw new NotImplementedException();
		}
		

		public IUpdateLog Get(string id)
		{
			return Query(new UpdatesLogQuery
				{
					After = id,
					Before = id,
					FromBoundary = BoundaryType.Inclusive,
					ToBoundary = BoundaryType.Inclusive
				})
				.FirstOrDefault();
		}

		public IEnumerable<IUpdateLog> GetAfter(string id, int count)
		{
			return Query(new UpdatesLogQuery
				{
					After = id,
					Count = 1
				});
		}

		public IEnumerable<IUpdateLog> GetBetween(string id, string before, int count)
		{
			return Query(new UpdatesLogQuery
				{
					After = id,
					Before = before,
					Count = 1
				});
		}

		public IEnumerable<IUpdateLog> Query(UpdatesLogQuery query)
		{
			throw new NotImplementedException();
		}
	}
}