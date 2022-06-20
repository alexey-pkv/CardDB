using System.Threading.Tasks;
using CardDB.Modules.APIModule.Models.Updates;
using CardDB.Modules.APIModule.Utils;
using CardDB.Modules.UpdatesLog;
using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Controller
{
	public static class UpdatesController
	{
		[ParameterRoute(HttpMethod.GET, "/updates")]
		public static async Task GetView(HttpContext ctx)
		{
			var module = Container.GetModule<IUpdatesLogModule>();
			var query = new UpdatesLogQuery
				{
					Count = ctx.GetIntParam("count", 0, 1000, 100),
					CardFilter = ctx.GetID("card"),
					ViewFilter = ctx.GetID("view"),
					After = ctx.GetID("after"),
					Before = ctx.GetID("before"),
					IsAscending = ctx.GetIntParam("descending", 0, 1, 0) != 1,
					AfterBoundary = ctx.GetIntParam("afterExclusive", 0, 1, 1) == 1 ? BoundaryType.Exclusive : BoundaryType.Inclusive,
					BeforeBoundary = ctx.GetIntParam("beforeExclusive", 0, 1, 1) == 1 ? BoundaryType.Exclusive : BoundaryType.Inclusive,
				};
			
			var result = module.Query(query);
			
			await ctx.Response.WithJSON(new UpdateLogSetModel(result));
		}
	}
}