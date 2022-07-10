using System;
using System.Threading.Tasks;
using CardDB;
using CardDB.Modules.PersistenceModule.DAO;
using CardDB.Modules.PersistenceModule.Models;
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
			
			
			a.ActionType = ActionType.DeleteCard;
			a.GeneratedID = await IDGenerator.Generate();
			
			await conn.Action.Save(a);
			
			return;
			
		}
	}
}