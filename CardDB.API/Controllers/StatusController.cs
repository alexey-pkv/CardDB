using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace CardDB.API.Controllers
{
	public class StatusController
	{
		public static async Task IndexAction(HttpContext context)
		{
			await context.Response.WriteAsync("ok");
		}
	}
}