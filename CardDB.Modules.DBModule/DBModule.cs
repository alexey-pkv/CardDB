using System.Collections.Generic;
using System.Threading.Tasks;
using Library;
using Library.State;

using CardDB.Engine;
using CardDB.Engine.Core;


namespace CardDB.Modules.DBModule
{
	public class DBModule : AbstractModule, IDBModule
	{
		private Dictionary<string, DBEngine> m_engines = new();


		public override string Name => "DB";

		
		private DBEngine CreateEngineObject(Bucket b)
		{
			DBEngine e = new DBEngine(b);
			
			var logs = GetModule<IUpdatesLogModule>();
			var persist = GetModule<IPersistenceModule>();
			
			e.Start(
				actionConsumers:	new IUpdatesConsumer[]{ logs, persist },
				indexConsumers:		new IUpdatesConsumer[]{ logs },
				p:					persist);
			
			m_engines.Add(b.ID, e);
			
			return e;
		}
		
		
		
		public DBEngine GetEngine(Bucket bucket)
		{
			DBEngine e;
			
			lock (m_engines)
			{
				m_engines.TryGetValue(bucket.ID, out e);
			}
			
			return e;
		}
		
		public DBEngine GetOrCreateEngine(Bucket bucket)
		{
			DBEngine e;
			
			lock (m_engines)
			{
				if (!m_engines.TryGetValue(bucket.ID, out e))
				{
					e = CreateEngineObject(bucket);
				}
			}
			
			return e;
		}
		
		public DBEngine CreateEngine(Bucket bucket)
		{
			DBEngine e;
			
			lock (m_engines)
			{
				if (m_engines.ContainsKey(bucket.ID))
					throw new CardDBException($"Bucket {bucket} already exists!");
				
				e = CreateEngineObject(bucket);
			}
			
			return e;
		}
		

		public DB GetDB(Bucket bucket)
		{
			return GetEngine(bucket)?.DB;
		}
		
		public DB GetOrCreateDB(Bucket bucket)
		{
			return GetOrCreateEngine(bucket)?.DB;
		}
		
		public DB CreateDB(Bucket bucket)
		{
			return CreateEngine(bucket)?.DB;
		}
		
		
		public bool TryGetView(Bucket b, string id, out Card view)
		{
			var db = GetDB(b);
			
			if (db != null)
			{
				return db.TryGetView(id, out view);
			}
			else
			{
				view = null;
				return false;
			}
		}
		
		public async Task AddAction(Bucket b, Action action)
		{
			var e = GetOrCreateEngine(b);
			
			action.BucketID = b.ID;
			
			await e.AddAction(action);
		}
		
		
		public override void PreStop(IStateManager state)
		{
			List<Task> stopTasks = new List<Task>();  
			
			lock (m_engines)
			{
				foreach (var value in m_engines.Values)
				{
					stopTasks.Add(value.Stop());
				}
				
				m_engines.Clear();
			}
			
			state.CompleteAfter(stopTasks);
		}
	}
}