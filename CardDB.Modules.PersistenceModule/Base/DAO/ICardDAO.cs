using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CardDB.Modules.PersistenceModule.Base.DAO
{
	public interface ICardDAO
	{
		public Task Insert(Card c);
		public Task Update(Card c);
		public Task UpdateAll(IEnumerable<Card> c);
		public Task<Card> Load(string id);
		public Task<Card[]> LoadAll(string after = null);
		public Task LoadAll(Func<Card[], Task> callback);
	}
}