using System.Threading.Tasks;
using CardDB.Engine;
using Microsoft.AspNetCore.Http;


namespace CardDB.API.Controllers
{
	public class CardController
	{
		public static async Task CreateAction(DBEngine engine, HttpContext context)
		{
			
		}
		
		public static async Task GetAction(DBEngine engine, HttpContext context)
		{
			if (!context.Request.Query.TryGetValue("id", out var id) || 
			    id.Count > 10 || 
			    id.Count < 4)
			{
				context.Response.StatusCode = 400;
				await context.Response.WriteAsync("missing id");
				return;
			}
			
			if (!engine.DB.Cards.Cards.TryGetValue(id, out var card))
			{
				context.Response.StatusCode = 404;
				await context.Response.WriteAsync("{}");
				return;
			}
			
			
		}
		
		public static async Task UpdateAction(DBEngine engine, HttpContext context)
		{
			await context.Response.WriteAsync("ok");
		}
	}
}