using System;
using System.Collections.Generic;


namespace CardDB
{
	public class Card : AbstractDBEntity
	{
		public ulong SequenceID { get; set; }
		public bool IsDeleted { get; set; } = false;
		public Dictionary<string, string> Properties { get; } = new();
		
		
		public void SetProperty(string key, string value) => Properties[key] = value;
		public void DeleteProperty(string key) => Properties.Remove(key);
		
		
		public void UpdateSequence(ulong to)
		{
			SequenceID = to > SequenceID ? to : SequenceID;
		}
		
		
		public void Delete()
		{
			IsDeleted = true;
			Properties.Clear();
		}
	}
}