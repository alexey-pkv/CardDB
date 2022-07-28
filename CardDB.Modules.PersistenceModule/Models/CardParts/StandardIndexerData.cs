using System.Text.Json.Serialization;

using CardDB.Indexing;


namespace CardDB.Modules.PersistenceModule.Models.CardParts
{
	public class StandardIndexerData
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string[] order { get; set; }
		
		public ConditionModel condition { get; set; }
		
		
		public StandardIndexerData() {}
		
		public StandardIndexerData(StandardIndexer indexer)
		{
			if (indexer.OrderProperties.Length > 0)
				order = indexer.OrderProperties;
			
			if (indexer.Condition != null)
				condition = new ConditionModel(indexer.Condition);
		}
		
		
		public StandardIndexer Get()
		{
			return new StandardIndexer
			{
				Condition = condition.Get(),
				OrderProperties = order
			};
		}
	}
}