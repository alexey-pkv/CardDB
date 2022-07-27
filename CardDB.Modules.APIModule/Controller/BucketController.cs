using System.Threading.Tasks;
using CardDB.Modules.APIModule.Models;
using CardDB.Modules.APIModule.Utils;
using CardDB.Modules.PersistenceModule.Exceptions;
using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Controller
{
	public static class BucketController
	{
		[ParameterRoute(HttpMethod.GET, "/bucket/{name}")]
		public static async Task GetBucket(HttpContext ctx)
		{
			var module = Container.GetModule<IPersistenceModule>();
			var name = ctx.GetURLString("name");
			
			if (name == null || !Formats.TrySanitizeBucketName(name, out name))
			{
				await ctx.Response.WithError(400, $"Invalid name {name ?? "null"}");
				return;
			}
			
			if (name == null)
			{
				await ctx.Response.WithInvalidParameter("name");
				return;
			}
			
			var bucket = await module.GetByName(name);
			
			if (bucket == null)
			{
				await ctx.Response.WithNotFound();
				return;
			}
			
			await ctx.Response.WithJSON(new BucketModel(bucket));
		}
		
		[ParameterRoute(HttpMethod.POST, "/bucket/{name}")]
		public static async Task CreateBucket(HttpContext ctx)
		{
			var module = Container.GetModule<IPersistenceModule>();
			var name = ctx.GetURLString("name");
			Bucket bucket;
			
			if (name == null || !Formats.TrySanitizeBucketName(name, out name))
			{
				await ctx.Response.WithError(400, $"Invalid name {name ?? "null"}");
				return;
			}
			
			try
			{
				bucket = await module.Create(name);
			}
			catch (BucketAlreadyExistsException e)
			{
				await ctx.Response.WithError(400, e.Message);
				return;
			}
			
			await ctx.Response.WithJSON(new BucketModel(bucket));
		}
	}
}