namespace CardDB
{
	public class Property
	{
		public Field Field { get; set; }
		public object Value { get; set; } = null;
		
		
		public FieldType Type => Field.Type;
		public string FieldID => Field.ID; 
	}
}