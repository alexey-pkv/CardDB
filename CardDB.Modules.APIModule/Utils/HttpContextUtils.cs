using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CardDB.Modules.APIModule.Models;
using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Utils
{
	public static class HttpContextUtils
	{
		private static readonly Regex ID_REGEX = new Regex("^[a-z0-9]{12}$");
		
		
		public static string GetID(this HttpContext ctx, string param = "id", string def = null)
		{
			if (!ctx.Request.Url.Parameters.TryGetValue(param, out var val))
			{
				return def;
			}
			
			if (!ID_REGEX.IsMatch(val))
			{
				return def;
			}
			
			return val;
		}
		
		public static async Task<JsonElement> ReadJSON(this HttpContext ctx)
		{
			return await ReadJSONBody<JsonElement>(ctx);
		}
		
		public static async Task<T> TryReadJSONBody<T>(this HttpContext ctx, T def = default)
		{
			try
			{
				return await ctx.ReadJSONBody<T>();
			}
			catch (JsonException e)
			{
				Log.Fatal("Failed to parse to JSON body", e);
				return def;
			}
		}
		
		public static async Task<T> ReadJSONBody<T>(this HttpContext ctx)
		{
			StreamReader reader = new StreamReader(ctx.Request.Data, Encoding.UTF8, true, 1024, true);
			var body = await reader.ReadToEndAsync();
			
			return JSON.Deserialize<T>(body);
		}
		
		public static async Task<CreateCardModel> GetCreateCard(this HttpContext ctx)
		{
			Dictionary<string, string> props = await ReadJSONBody<Dictionary<string, string>>(ctx);
			
			if (props.Count > 1000)
				return null;
			
			return new CreateCardModel
			{
				properties = props
			};
		}
		
		public static async Task<CreateViewModel> GetCreateView(this HttpContext ctx)
		{
			return await ReadJSONBody<CreateViewModel>(ctx);
		}
	}
}