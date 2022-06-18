namespace CardDB.Modules.UpdatesLog
{
	public class UpdatesLogQuery
	{
		public string After { get; set; } = null;
		public string Before { get; set; } = null;
		public BoundaryType FromBoundary { get; set; } = BoundaryType.Exclusive;
		public BoundaryType ToBoundary { get; set; } = BoundaryType.Exclusive;
		public bool IsAscending { get; set; } = true;
		public string ViewFilter { get; set; } = null;
		public string CardFilter { get; set; } = null;
		public int Count { get; set; } = 1;
	}
}