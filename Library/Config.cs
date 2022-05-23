using System;
using System.Collections.Generic;


namespace Library
{
	public class Config : IConfig
	{
		public class ConfigValue : IConfig.IConfigValue
		{
			private string m_val;
			public ConfigValue(string value) { m_val = value; }
			public bool GetBool()
			{
				var val = m_val.Trim().ToLower();
				return val == "on" || val == "1" || val == "true";
			}
			public int GetInt() => Int32.Parse(m_val);
			public float GetFloat() => float.Parse(m_val);
			public string Get() => m_val;
			public override string ToString() => m_val;
		}
		
		private Dictionary<string, string> m_values = new Dictionary<string, string>();
		
		
		public IConfig.IConfigValue this[string index]
		{
			get
			{
				if (!Has(index))
					throw new Exception($"Key \"{index}\" does not exist in the config");
				
				return new ConfigValue(m_values[index]);
			}
		}
		
		
		public bool GetBool(string key, bool? def = null) => (def != null && !Has(key) ? (bool)def : this[key].GetBool());
		public int GetInt(string key, int? def = null) => (def != null && !Has(key) ? (int)def : this[key].GetInt());
		public float GetFloat(string key, float? def = null) => ((def != null && !Has(key)) ? (float)def : this[key].GetFloat());
		public string Get(string key, string def = null) => ((def != null && !Has(key)) ? def : this[key].Get());
		
		public bool Has(string key)
		{
			return m_values.ContainsKey(key);
		}
		
		
		public void Set(string key, string value)
		{
			m_values[key] = value;
		}
	}
}