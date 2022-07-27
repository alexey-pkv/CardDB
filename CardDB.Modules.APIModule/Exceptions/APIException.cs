using System.Text.RegularExpressions;
using Library;


namespace CardDB.Modules.APIModule.Exceptions
{
	public class APIException : CardDBException
	{
		public int Code { get; set; }
		
		
		public APIException(string message, int code) : base(message)
		{
			Code = code;
		}
	}
	
	public class APIMissingBucketNameException : APIException
	{
		public APIMissingBucketNameException() 
			: base("Bucket Name is required for this request. Provide query param bucket-name or header x-bucket-name", 400) {}
	}
	
	public class APIInvalidBucketNameException : APIException
	{
		public APIInvalidBucketNameException(string val) 
			: base($"Value `{val}` is not a well formatted bucket name", 400) {}
	}
	
	public class APIBucketNotFoundByNameException : APIException
	{
		public APIBucketNotFoundByNameException(string name) 
			: base($"Bucket named `{name}` was not found", 400) {}
	}
}