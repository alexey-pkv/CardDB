using System.Threading.Tasks;
using System.Collections.Generic;
using CardDB.Updates;
using Library.ID;


namespace CardDB
{
	public class Action
	{
		public ulong Sequence { get; set; }
		public ActionType ActionType { get; set; }
		
		
		public Dictionary<string, string> Properties { get; set; } = null;
		public HashSet<string> DeletedProperties { get; set; } = null;
		
		public HashSet<string> CardIDs { get; set; } = null;
		public string ViewID { get; set; } = null;
		public IIndexer ViewIndex { get; set; }
		public ICondition UpdateCondition { get; set; }
			
		
		private CardUpdate ApplyDeleteCard(Card c)
		{
			c.Delete();
			
			return CardUpdate.Deleted(this, c);
		}
		
		private CardUpdate ApplyModifyCard(Card c)
		{
			Dictionary<string, string> previous = null;
			Dictionary<string, string> current = null;
			
			foreach (var name in DeletedProperties)
			{
				if (c.Properties.TryGetValue(name, out var val))
				{
					previous ??= new();
					previous.Add(name, val);
					c.DeleteProperty(name);
				}
			}
			
			foreach (var kvp in Properties)
			{
				if (!c.Properties.TryGetValue(kvp.Key, out var val))
				{
					current ??= new ();
					
					current.Add(kvp.Key, kvp.Value);
					c.SetProperty(kvp.Key, kvp.Value);
				}
				else if (val != kvp.Value)
				{
					previous ??= new();
					current ??= new ();
					
					previous.Add(kvp.Key, val);
					current.Add(kvp.Key, kvp.Value);
					c.SetProperty(kvp.Key, kvp.Value);
				}
				
			}
			
			if (previous == null || current == null)
			{
				return null;
			}
			
			return new CardUpdate
			{
				Card = c,
				NewProperties = current,
				PreviousProperties = previous,
				UpdateType = UpdateType.Modified
			};
		}
		
		
		public bool IsCardAction =>
			ActionType == ActionType.CreateCard ||
			ActionType == ActionType.DeleteCard || 
			ActionType == ActionType.ModifyCard; 
		
		public bool IsAffecting(Card c)
		{
			if (c.IsDeleted || CardIDs == null)
				return false;
			
			return CardIDs.Contains(c.ID);
		}
		
		public CardUpdate UpdateCard(Card c)
		{
			c.UpdateSequence(Sequence);
			
			if (!IsAffecting(c))
				return null;
			
			if (UpdateCondition != null && !UpdateCondition.IsMatching(c))
				return null;
			
			if (ActionType == ActionType.DeleteCard)
			{
				return ApplyDeleteCard(c);
			}
			else if (ActionType == ActionType.ModifyCard)
			{
				return ApplyModifyCard(c);
			}
			
			return null;
		}
		
		public async Task<CardUpdate> CreateCard()
		{
			Card c = new(await IDGenerator.Generate());

			foreach (var kvp in Properties)
			{
				c.Properties.Add(kvp.Key, kvp.Value);
			}
			
			return CardUpdate.CardCreated(this, c);
		}
	}
}