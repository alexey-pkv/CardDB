using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface IItemDAO
	{
		public Task Insert(Card c);
		public Task Update(Card c);
	}
}