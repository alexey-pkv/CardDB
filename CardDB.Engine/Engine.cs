using System.Threading.Tasks;
using CardDB.Engine.Core;
using CardDB.Engine.Operators;
using CardDB.Engine.StartupData;


namespace CardDB.Engine
{
	public class Engine
	{
		#region Private Data Members
		
		private IActionPersistence m_persistence = null;
		
		private DB m_db = new();
		private ActionsOperator m_actions = new();
		private ReIndexOperator m_indexer = new();
		
		#endregion
		
		#region Properties
		
		public DB DB => m_db;
		
		#endregion
		
		#region Constructor
		
		public Engine()
		{
			m_persistence = new MemoryActionPersistence();
			
			m_indexer.Setup(new ReIndexOperatorStartupData
			{
				DB			= m_db,
				Consumer	= null,
			});
			
			m_actions.Setup(new ActionsOperatorStartupData
			{
				DB				= m_db,
				Actions			= null,
				UpdatesConsumer	= m_indexer,
				LastSequenceID	= 0
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
		
		public void Start()
		{
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