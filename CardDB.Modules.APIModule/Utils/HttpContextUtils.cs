using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using CardDB.Modules.APIModule.Models;
using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Utils
{
	public static class HttpContextUtils
	{
		private static readonly Regex ID_REGEX = new("^[a-z0-9]{12}$");
		
		
		public static Card GetView(this HttpContext ctx, string param = "id")
		{
			var id = ctx.GetID(param);
			
			if (id == null)
			{
				return null;
			}
			
			var module = Container.GetModule<IDBModule>();
			
			return module.Engine.TryGetView(id, out var view) ? view : null;
		}
		
		
		public static string GetStringParam(this HttpContext ctx, string param = "id", string def = null)
		{
			if (!ctx.Request.Query.Elements.TryGetValue(param, out var val))
			{
				return def;
			}
			
			val = HttpUtility.UrlDecode(val);
			
			return val;
		}
		
		public static bool GetBoolParam(this HttpContext ctx, string param, bool def = false)
		{
			var res = ctx.GetIntParam(param, def: def ? 1 : 0);
			
			return (res == 0 ? false : true);
		}
		
		public static int GetIntParam(this HttpContext ctx, string param = "id", 
			int min = int.MinValue, int max = int.MaxValue, int def = 0)
		{
			if (!ctx.Request.Query.Elements.TryGetValue(param, out var val))
			{
				return def;
			}
			
			if (!int.TryParse(val, out var int_val))
			{
				return def;
			}
			
			if (int_val < min || int_val > max)
			{
				return def;
			}
			
			return int_val;
		}
		
		
		public static string GetURLString(this HttpContext ctx, string param, string def = null)
		{
			if (!ctx.Request.Url.Parameters.TryGetValue(param, out var val))
			{
				return def;
			}
			
			return val;
		}
		
		public static string GetID(this HttpContext ctx, string param = "id", string def = null)
		{
			if (!ctx.Request.Url.Parameters.TryGetValue(param, out var val) && 
			    !ctx.Request.Query.Elements.TryGetValue(param, out val))
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