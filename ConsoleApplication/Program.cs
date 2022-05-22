using System.Collections.Generic;
using System.Threading.Tasks;
using CardDB;
using CardDB.Conditions.ValueConditions;
using CardDB.Engine;
using CardDB.Indexing;


namespace ConsoleApplication
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Engine e = new Engine();
			
			e.Start();
			
			var condition = new ContainsValueCondition() { Field = "hello", Value = "world" };
			var indexer = new StandardIndexer { Condition = condition, OrderProperties = new []{"ord"} };
			
			var aView = new Action
			{
				ActionType = ActionType.CreateView,
				ViewIndex = indexer
			};
			
			var aCard = new Action
			{
				ActionType	= ActionType.CreateCard,
				Properties	= new()
				{
					{ "hello", "world" },
					{ "good by", "city" },
				}
			};
			
			await e.AddAction(aView);
			await e.AddAction(aCard);
			
			await Task.Delay(1000000);
		}
	}
}