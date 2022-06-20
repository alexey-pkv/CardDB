using System.Threading.Tasks;

using CardDB.Engine.Core;
using CardDB.Engine.Operators;
using CardDB.Engine.StartupData;


namespace CardDB.Engine
{
	public class DBEngine
	{
		#region Private Data Members
		
		private IActionPersistence m_persistence;
		
		private DB m_db = new();
		private ActionsOperator m_actions = new();
		private ReIndexOperator m_indexer = new();

		#endregion
		
		#region Properties
		
		public DB DB => m_db;
		
		#endregion
		
		#region Private Methods
		
		private void Init(IUpdatesConsumer logs)
		{
			m_persistence = new MemoryActionPersistence();
						
			m_indexer.Setup(new ReIndexOperatorStartupData
			{
				DB			= m_db,
				Consumer	= logs
			});
			
			m_actions.Setup(new ActionsOperatorStartupData
			{
				DB				= m_db,
				Actions			= null,
				LastSequenceID	= 0,
				UpdatesConsumer	= new ConsumersCollection(new []
				{
					logs,
					m_indexer
				})
			});
		}
		
		#endregion
		
		#region Public Methods
		
		public async Task AddAction(Action action)
		{
			await m_persistence.Persist(action, a => m_actions.AddAction(a));
		}
		
		public void ForceUpdate(Card c)
		{
			m_actions.ForceUpdateCard(c);
		}
		
		public async Task ForceUpdate(Card c, View v)
		{
			ForceUpdate(c);
			await m_indexer.Index(c, v);
		}
		
		public void Start(IUpdatesConsumer log)
		{
			Init(log);
			
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
		
		public bool TryGetView(string id, out View view)
		{
			return DB.Views.Views.TryGetValue(id, out view);
		}
		
		#endregion
	}
}