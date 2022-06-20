namespace CardDB
{
	public class UpdateLog
	{
		public string RecordID { get; init; }
		public IUpdate Update { get; init; }
	}
}