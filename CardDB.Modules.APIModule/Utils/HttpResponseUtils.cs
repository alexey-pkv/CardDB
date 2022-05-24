using System.Threading;
using System.Threading.Tasks;
using CardDB.Modules.APIModule.Models;

using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule.Utils
{
	public static class HttpResponseUtils
	{
		public static async Task<bool> WithJSON(this HttpResponse response, object with, CancellationToken token = default)
		{
			if (with is AbstractWithCodeModel)
			{
				response.StatusCode = ((AbstractWithCodeModel)with).Code;
			}
			
			return await response.Send(JSON.Serialize(with), token);
		}
		
		public static async Task WithNotFound(this HttpResponse response)
		{
			await response.WithJSON(new NotFoundErrorModel());
		}
		
		public static async Task WithInvalidParameter(this HttpResponse response, string param)
		{
			await response.WithJSON(new InvalidParamErrorModel(param));
		}
		
		public static async Task WithOK(this HttpResponse response)
		{
			response.StatusCode = 200;
			await response.Send();
		}
		
		public static async Task WithInvalidInputFormat(this HttpResponse response, string expected)
		{
			await response.WithJSON(new InvalidInputFormatErrorModel(expected));
		}
		
		public static async Task WithID(this HttpResponse response, string id)
		{
			await response.WithJSON(new IDModel { id = id });
		}
	}
}