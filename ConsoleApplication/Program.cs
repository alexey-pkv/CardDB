using System.Collections.Generic;
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
			
			
			a.ActionType = ActionType.DeleteCard;
			a.GeneratedID = await IDGenerator.Generate();
			a.Properties = new Dictionary<string, string>
			{
				{ "A" , "hello world"}	
			};
			
			await conn.Action.Save(a);
			
			// var res= conn.Action.Load(0, 10);
			
			return;
			
		}
	}
}