using System.Collections.Generic;
using Library.ID;


namespace CardDB
{
	public class FieldsSet
	{
		public Dictionary<string, Field> Fields { get; } = new();
		
		
		public void AddField(Field field)
		{
			field.ID = IDGenerator.Generate();
			Fields[field.ID] = field;
		}
		
		
		public void RemoveField(Card card) => RemoveField(card.ID);
		public void RemoveField(string id) => Fields.Remove(id);
	}
}