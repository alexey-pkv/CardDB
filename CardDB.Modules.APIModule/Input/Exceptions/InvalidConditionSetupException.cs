namespace CardDB.Modules.APIModule.Input.Exceptions
{
	public class InvalidConditionSetupException : InputException
	{
		public InvalidConditionSetupException(string message, string path) : base($"At {path}: {message}")
		{
			
		}
	}
}