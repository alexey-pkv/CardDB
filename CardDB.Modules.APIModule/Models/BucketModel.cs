namespace CardDB.Modules.APIModule.Models
{
	public class BucketModel
	{
		public string id { get; }
		public string name { get; }
		
		
		public BucketModel(Bucket b)
		{
			id = b.ID;
			name = b.Name;
		}
	}
}