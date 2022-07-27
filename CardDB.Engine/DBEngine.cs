using System;
using System.Linq;
using System.Threading.Tasks;

using CardDB.Engine.Core;
using CardDB.Engine.Operators;
using CardDB.Engine.StartupData;

using Library;


namespace CardDB.Engine
{
	public class DBEngine
	{
		#region Private Data Members
		
		private IActionPersistence m_persistence;
		
		private DB m_db;
		private ActionsOperator m_actions = new();
		private ReIndexOperator m_indexer = new();

		#endregion
		
		#region Properties
		
		public DB DB => m_db;
		public Bucket Bucket { get; }
		
		#endregion
		
		#region Private Methods
		
		private IUpdatesConsumer GetSingleConsumer(IUpdatesConsumer[] source)
		{
			if (source == null || source.Length == 0)
				return null;
			else if (source.Length == 1)
				return source[0];
			else 
				return new ConsumersCollection(source);
		}
		
		private void Init(
			IUpdatesConsumer[] actionsConsumers, 
			IUpdatesConsumer[] indexConsumers, 
			IActionPersistence p)
		{
			m_persistence = p ?? new MemoryActionPersistence();
			
			if (actionsConsumers == null)
				actionsConsumers = new IUpdatesConsumer[]{ m_indexer };
			else 
				actionsConsumers = actionsConsumers.Append(m_indexer).ToArray();
			
			
			m_indexer.Setup(new ReIndexOperatorStartupData
			{
				DB			= m_db,
				Consumer	= GetSingleConsumer(indexConsumers)
			});
			
			m_actions.Setup(new ActionsOperatorStartupData
			{
				DB				= m_db,
				Actions			= null,
				LastSequenceID	= 0,
				UpdatesConsumer	= GetSingleConsumer(actionsConsumers)
			});
		}
		
		#endregion
		
		#region Constructor
		
		public DBEngine(Bucket b)
		{
			Bucket = b;
			m_db = new DB(b);
		}
		
		#endregion
		
		#region Public Methods
		
		public async Task AddAction(Action action)
		{
			try
			{
				await m_persistence.Persist(action, a => m_actions.AddAction(a));
			}
			catch (Exception e)
			{
				Log.Fatal("Error when creating action", e);
				throw;
			}
		}
		
		public void ForceUpdate(Card c)
		{
			m_actions.ForceUpdateCard(c);
		}
		
		public async Task ForceUpdate(Card c, Card v)
		{
			ForceUpdate(c);
			await m_indexer.Index(c, v);
		}
		
		public void Start(
			IUpdatesConsumer[] actionConsumers, 
			IUpdatesConsumer[] indexConsumers, 
			IActionPersistence p = null)
		{
			Init(actionConsumers, indexConsumers, p);
			
			m_indexer.Start();
			m_actions.StartConsumer();
		}
		
		public Task Stop()
		{
			return Task.WhenAll(
				m_indexer.Stop(),
				m_actions.StopConsumer()
			);
		}
		
		#endregion
	}
}