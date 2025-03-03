using System.Threading.Tasks;
using CardDB.Modules.APIModule.Models.Stats;
using CardDB.Modules.APIModule.Utils;
using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Controller
{
	public static class StatisticsController
	{
		[StaticRoute(HttpMethod.GET, "/stats")]
		public static async Task GetStatistics(HttpContext ctx)
		{
			var bucket = await ctx.RequireBucket();
			var module = Container.GetModule<IDBModule>();
			var db = module.GetDB(bucket);
			
			await ctx.Response.WithJSON(new StatsModel
			{
				cards = db.Cards.Count,
				views = db.Views.Count
			});
		}
	}
}