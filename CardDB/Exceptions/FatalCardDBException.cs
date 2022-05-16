using System;


namespace CardDB.Exceptions
{
	public class FatalCardDBException : Exception
	{
		public FatalCardDBException(string message)
			: base(message)
		{
			
		}
	}
}