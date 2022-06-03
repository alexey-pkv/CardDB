using System;
using System.Text.Json;
using System.Threading.Tasks;
using CardDB.Indexing;
using CardDB.Modules.APIModule.Input;
using CardDB.Modules.APIModule.Models;
using CardDB.Modules.APIModule.Utils;

using Library;
using Library.ID;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Controller
{
	public static class ViewController
	{
		[ParameterRoute(HttpMethod.GET, "/view/{id}")]
		public static async Task GetView(HttpContext ctx)
		{
			var id = ctx.GetID();
			
			if (id == null)
			{
				await ctx.Response.WithInvalidParameter("id");
				return;
			}
			
			var module = Container.GetModule<IDBModule>();
			
			if (module.Engine.DB.Views.Views.TryGetValue(id, out var view))
			{
				await ctx.Response.WithJSON(new ViewModel(view));
				return;
			}
			else
			{
				await ctx.Response.WithNotFound();
			}
		}
		
		[ParameterRoute(HttpMethod.POST, "/view")]
		public static async Task CreateView(HttpContext ctx)
		{
			var module = Container.GetModule<IDBModule>();
			CreateViewModel model;

			try
			{
				model = await ctx.GetCreateView();
			}
			catch (JsonException)
			{
				await ctx.Response.WithInvalidInputFormat("Failed to parse view configuration! Check your json.");
				return;
			}
			
			var id = await IDGenerator.Generate();
			ICondition cond;

			try
			{
				cond = IndexParser.Parse(model.index);
			}
			catch (InputException e)
			{
				await ctx.Response.WithInvalidInputFormat($"Failed to parse Condition setup: {e.Message}");
				return;
			}
			
			await module.Engine.AddAction(new Action
			{
				ActionType = ActionType.CreateView,
				GeneratedID = id,
				ViewIndex = new StandardIndexer
				{
					Condition = cond,
					OrderProperties = model.order
				}
			});
			
			await ctx.Response.WithID(id);
		}
	}
}