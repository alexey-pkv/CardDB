using Library;


namespace CardDB.Modules.APIModule.Input
{
	public class InputException : CardDBException
	{
		public InputException(string message) : base(message)
		{
		}
	}
}