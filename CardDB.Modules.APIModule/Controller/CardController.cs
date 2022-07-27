using System.Collections.Generic;
using System.Threading.Tasks;

using CardDB.Modules.APIModule.Models;
using CardDB.Modules.APIModule.Utils;

using Library;
using Library.ID;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Controller
{
	public static class CardController
	{
		[ParameterRoute(HttpMethod.GET, "/card/{id}")]
		public static async Task GetCard(HttpContext ctx)
		{
			var bucket = await ctx.RequireBucket();
			var id = ctx.GetID();
			var latest = ctx.GetBoolParam("latest");
			
			if (id == null)
			{
				await ctx.Response.WithInvalidParameter("id");
				return;
			}
			
			var module = Container.GetModule<IDBModule>();
			var engine = module.GetOrCreateEngine(bucket);
			var db = engine.DB;
			
			if (!db.Cards.Cards.TryGetValue(id, out var c))
			{
				await ctx.Response.WithNotFound();
				return;
			}
			
			if (latest)
			{
				engine.ForceUpdate(c);
			}
			
			if (c.IsDeleted)
			{
				await ctx.Response.WithNotFound();
				return;
			}
			
			await ctx.Response.WithJSON(new CardModel(c));
		}
		
		[ParameterRoute(HttpMethod.POST, "/card")]
		public static async Task CreateCard(HttpContext ctx)
		{
			var bucket = await ctx.RequireBucket();
			var module = Container.GetModule<IDBModule>();
			var createCard = await ctx.GetCreateCard();
			
			if (createCard == null)
			{
				await ctx.Response.WithInvalidInputFormat("card");
				return;
			}
			
			var id = await IDGenerator.Generate();
			
			await module.AddAction(
				bucket,
				new Action
				{
					GeneratedID = id,
					ActionType = ActionType.CreateCard,
					Properties = createCard.properties,
				});
			
			await ctx.Response.WithID(id);
		}
		
		[ParameterRoute(HttpMethod.DELETE, "/card/{id}")]
		public static async Task DeleteCard(HttpContext ctx)
		{
			var bucket = await ctx.RequireBucket();
			var id = ctx.GetID();
			
			if (id == null)
			{
				await ctx.Response.WithInvalidParameter("id");
				return;
			}
			
			var module = Container.GetModule<IDBModule>();
			
			await module.AddAction(
				bucket,
				new Action
				{
					CardIDs = new HashSet<string>(new [] { id }),
					ActionType = ActionType.DeleteCard,
				});
			
			await ctx.Response.WithOK();
		}
	}
}