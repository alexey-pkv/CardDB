using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface ICardDAO
	{
		public Task Insert(Card c);
		public Task Update(Card c);
	}
}