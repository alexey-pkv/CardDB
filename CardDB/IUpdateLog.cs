namespace CardDB
{
	public interface IUpdateLog : IUpdate
	{
		public string RecordID { get; set; }
	}
}