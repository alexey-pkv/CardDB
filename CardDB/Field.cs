namespace CardDB
{
	public class Field : AbstractDBEntity
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		
		public FieldType Type { get; set; }
		public bool IsNullable { get; set; } = true;
		public object DefaultValue { get; set; } = null;
		public string[] EnumSet { get; set; } = null;
	}
}