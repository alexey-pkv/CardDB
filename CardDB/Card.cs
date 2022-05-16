using System;
using System.Collections.Generic;


namespace CardDB
{
	public class Card : AbstractDBEntity
	{
		public Int64 SequenceID { get; set; }
		public Dictionary<string, Property> Properties { get; } = new();
		
		
		public void SetProperty(Field field, object value) => SetProperty(new Property { Field = field, Value = value });
		public void SetProperty(Property prop) => Properties[prop.FieldID] = prop;
		
		public void DeleteProperty(Property prop) => DeleteProperty(prop.FieldID);
		public void DeleteProperty(Field field) => DeleteProperty(field.ID);
		public void DeleteProperty(string id) => Properties.Remove(id);
	}
}