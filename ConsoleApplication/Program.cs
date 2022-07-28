using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardDB;
using CardDB.Modules.PersistenceModule.DAO;
using Library;
using Library.ID;
using Action = CardDB.Action;


namespace ConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var conn = new Connector(new Config());
			var a = new Action();
			
			
			var c = new Connector(new Config());
			var dao = new CardDAO(c);
			
			var card = await dao.Load("tdwuxid7m5t0");
			
			card.Properties["asdsa"] = "asdasdas";
			card.SequenceID = 781;
			
			await dao.Update(card);
			
			return;
			
		}
	}
}