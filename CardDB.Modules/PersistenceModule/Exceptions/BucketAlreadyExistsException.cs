using Library;

namespace CardDB.Modules.PersistenceModule.Exceptions
{
	public class BucketAlreadyExistsException : CardDBException
	{
		public BucketAlreadyExistsException(string name) : base($"The name {name} is already taken")
		{
		}
	}
}