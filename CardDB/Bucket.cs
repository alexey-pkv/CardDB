namespace CardDB
{
	public class Bucket
	{
		public string ID { get; set; }
		public string Name { get; set; }


		public override string ToString()
		{
			return $"<{ID}:{Name}>";
		}
	}
}