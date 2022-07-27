using Library;
using WatsonWebserver;


namespace CardDB.Modules.APIModule
{
	public class APIErrorHandler
	{
		public static void Handle(object? sender, ExceptionEventArgs args)
		{
			Log.Error($"[APIModule] Failed to handle request {args.Method}: {args.Url}", args.Exception);
		}
	}
}