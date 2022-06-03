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
			var id = ctx.GetID();
			
			if (id == null)
			{
				await ctx.Response.WithInvalidParameter("id");
				return;
			}
			
			var module = Container.GetModule<IDBModule>();
			var db = module.Engine.DB;
			
			if (!db.Cards.Cards.TryGetValue(id, out var c))
			{
				await ctx.Response.WithNotFound();
				return;
			}
			
			await ctx.Response.WithJSON(new CardModel(c));
		}
		
		[ParameterRoute(HttpMethod.POST, "/card")]
		public static async Task CreateCard(HttpContext ctx)
		{
			var module = Container.GetModule<IDBModule>();
			var createCard = await ctx.GetCreateCard();
			
			if (createCard == null)
			{
				await ctx.Response.WithInvalidInputFormat("card");
				return;
			}
			
			var id = await IDGenerator.Generate();
			
			await module.Engine.AddAction(new Action
			{
				GeneratedID = id,
				ActionType = ActionType.CreateCard,
				Properties = createCard.properties,
			});
			
			await ctx.Response.WithID(id);
		}
	}
}