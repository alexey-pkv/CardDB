using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace CardDB.Modules.PersistenceModule.DAO
{
	public class AbstractDAO
	{
		private IMySQLConnectionProvider m_provider;
		
		
		protected Task<MySqlConnection> Connection() => m_provider.Get();
		protected Task<MySqlCommand> Command() => m_provider.GetCommand();
		
		
		public AbstractDAO(IMySQLConnectionProvider connectionProvider)
		{
			m_provider = connectionProvider;
		}
	}
}