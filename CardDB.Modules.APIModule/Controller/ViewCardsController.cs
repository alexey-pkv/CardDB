using System.Threading.Tasks;
using CardDB.Modules.APIModule.Models;
using CardDB.Modules.APIModule.Utils;

using WatsonWebserver;


namespace CardDB.Modules.APIModule.Controller
{
	public static class ViewCardsController
	{
		[ParameterRoute(HttpMethod.GET, "/view/{id}/cards")]
		public static async Task GetCards(HttpContext ctx)
		{
			var view = ctx.GetView();
			var count = ctx.GetIntParam(
				param: "count",
				def: 100,
				min: 1,
				max: 100);
			
			var afterParam = ctx.GetStringParam("after");
			CardIndex after = null;
			
			if (!string.IsNullOrEmpty(afterParam))
			{
				after = CardIndex.FromJSON(afterParam);
			}
			
			if (view == null)
			{
				await ctx.Response.WithNotFound();
			}
			
			var list = view.GetList(count, after);
			
			await ctx.Response.WithJSON(new CardIndexSetModel(list));
		}
	}
}