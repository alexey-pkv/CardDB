using System;
using System.Text.Json.Serialization;
using CardDB.Engine.Operators.ReIndex;
using CardDB.Indexing;


namespace CardDB.Modules.PersistenceModule.Models.ActionParts
{
	public class IndexerData
	{
		private const string STANDARD_INDEXER = "standard_indexer";
		
		
		public string type { get; set; }
		
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string[] order_properties { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ConditionData condition { get; set; }
		
		
		public IndexerData(IIndexer indexer)
		{
			if (indexer is StandardIndexer)
			{
				var i = (StandardIndexer)indexer;
				
				type = STANDARD_INDEXER;
				
				if (i.OrderProperties != null)
					order_properties = i.OrderProperties;
				
				if (i.Condition != null)
					condition = new ConditionData(i.Condition);
			}
			else
			{
				throw new InvalidOperationException("Missing config for indexer of type " + indexer.GetType());
			}
		}
		
		
		public void Setup(Action a)
		{
			IIndexer indexer;
			
			switch (type)
			{
				case STANDARD_INDEXER:
					indexer = new StandardIndexer
					{
						Condition = condition?.Create(),
						OrderProperties = order_properties
					};
					break;
				
				default:
					throw new InvalidOperationException($"Unexpected type {type}");
			}
			
			a.ViewIndex = indexer;
		}
	}
}