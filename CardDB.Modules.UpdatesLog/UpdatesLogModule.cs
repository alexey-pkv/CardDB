using System;
using System.Linq;
using System.Collections.Generic;
using CardDB.Updates;
using Library;
using Library.ID;


namespace CardDB.Modules.UpdatesLog
{
	public class UpdatesLogModule : AbstractModule, IUpdatesLogModule
	{
		public override string Name => "UpdatesLog";
		
		
		private ulong m_lastID = 0;
		private List<UpdateLog> m_updates = new(10_000);
		
		
		private IEnumerable<UpdateLog> QuerySync(UpdatesLogQuery query)
		{
			List<UpdateLog> result = new();
			
			foreach (var update in m_updates)
			{
				var range = GetRange(update, query);

				if (range < 0)
				{
					continue;
				}
				else if (range == 0)
				{
					if (!IsMatchingFilter(update, query))
						continue;
					
					result.Add(update);
				
					if (result.Count >= query.Count)
					{
						break;
					}
				}
				else if (range > 0)
				{
					break;
				}
			}
			
			return result;
		}
		
		private int GetRange(UpdateLog log, UpdatesLogQuery query)
		{
			if (query.After != null)
			{
				var compare = String.CompareOrdinal(log.RecordID, query.After);
				
				if (compare < 0 || 
				    (compare == 0 && query.AfterBoundary == BoundaryType.Exclusive))
				{
					return -1;
				}
			}
			
			if (query.Before != null)
			{
				var compare = String.CompareOrdinal(log.RecordID, query.Before);
				
				if (compare > 0 || 
				    (compare == 0 && query.AfterBoundary == BoundaryType.Exclusive))
				{
					return 1;
				}
			}
			
			return 0;
		}
		
		private bool IsMatchingFilter(UpdateLog log, UpdatesLogQuery query)
		{
			var update = log.Update;
			
			if (query.CardFilter == null && 
			    query.ViewFilter == null)
			{
				return true;
			}
			
			if (update is CardUpdate && query.CardFilter != null)
			{
				return ((CardUpdate)update).Card.ID == query.CardFilter;
			}
			else if (update is IndexUpdate)
			{
				if (query.CardFilter != null && ((IndexUpdate)update).Card.ID != query.CardFilter)
				{
					return false;
				}
				
				if (query.ViewFilter != null && ((IndexUpdate)update).View.ID != query.ViewFilter)
				{
					return false;
				}
			}
			else if (update is ViewUpdate && query.ViewFilter != null)
			{
				return ((ViewUpdate)update).View.ID == query.ViewFilter;
			}
			
			return true;
		}
		
		
		public void Consume(IUpdate update)
		{
			lock (m_updates)
			{
				UpdateLog log = new UpdateLog
				{
					RecordID = IDUtils.ToID(m_lastID++),
					Update = update
				};
				
				if (m_updates.Count == m_updates.Capacity)
				{
					m_updates.RemoveAt(0);
				}
				
				m_updates.Add(log);
				
				Log.Info($"[UPDATE] Got: {log.RecordID} - {update.UpdateType} - {update.TargetType}");
			}
		}
		

		public UpdateLog Get(string id)
		{
			return Query(new UpdatesLogQuery
				{
					After = id,
					Before = id,
					AfterBoundary = BoundaryType.Inclusive,
					BeforeBoundary = BoundaryType.Inclusive
				})
				.FirstOrDefault();
		}

		public IEnumerable<UpdateLog> GetAfter(string id, int count)
		{
			return Query(new UpdatesLogQuery
				{
					After = id,
					Count = 1
				});
		}

		public IEnumerable<UpdateLog> GetBetween(string id, string before, int count)
		{
			return Query(new UpdatesLogQuery
				{
					After = id,
					Before = before,
					Count = 1
				});
		}
		
		
		public IEnumerable<UpdateLog> Query(UpdatesLogQuery query)
		{
			lock (m_updates)
			{
				return QuerySync(query);
			}
		}
	}
}