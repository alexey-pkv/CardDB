namespace Library
{
	public interface IConfig
	{
		public interface IConfigValue
		{
			public bool GetBool();
			public int GetInt();
			public float GetFloat();
			public string Get();
			public string ToString();
		}
		
		public IConfigValue this[string index] { get; }
		
		public bool GetBool(string key, bool? def = null);
		public int GetInt(string key, int? def = null);
		public float GetFloat(string key, float? def = null);
		public string Get(string key, string def = null);
		
		public bool Has(string key);
	}
}