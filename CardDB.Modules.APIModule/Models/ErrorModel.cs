namespace CardDB.Modules.APIModule.Models
{
	public class ErrorModel : AbstractWithCodeModel
	{
		public string message { get; init; }
	}
	
	
	public class NotFoundErrorModel : ErrorModel
	{
		public NotFoundErrorModel(string msg = "not found")
		{
			Code = 404;
			message = msg;
		}
	}
	
	public class InvalidParamErrorModel : ErrorModel
	{
		public InvalidParamErrorModel(string param)
		{
			Code = 400;
			message = $"Parameter [{param}] is either missing or invalid";
		}
	}
	
	public class InvalidInputFormatErrorModel : ErrorModel
	{
		public InvalidInputFormatErrorModel(string expectedModel)
		{
			Code = 400;
			message = $"Invalid input format for this endpoint. Expecting [{expectedModel}].";
		}
	}
}