using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Library.JSONConverters;


namespace Library
{
	public static class JSON
	{
		private static JsonSerializerOptions m_options = new JsonSerializerOptions();
		
		
		static JSON()
		{
			m_options.Converters.Add(new DateTimeConverter());
			m_options.Converters.Add(new IPConverter());
			m_options.IgnoreNullValues = true;
			
			
			var type = typeof(IAutoloadCacaoJSONConverter);
			var types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => p.IsAssignableTo(type) && 
					!p.IsInterface &&
					p.IsAssignableTo(typeof(JsonConverter)) &&
					p.GetConstructor(Type.EmptyTypes) != null);
			
			foreach (var i in types)
			{
				AddConverter(Activator.CreateInstance(i) as JsonConverter);
			}
		}
		
		
		public static JsonSerializerOptions Options => m_options;
		public static void AddConverter(JsonConverter converter) => m_options.Converters.Add(converter);
		public static void AddConverter<T>() where T : JsonConverter, new() => m_options.Converters.Add(new T());
		
		
		public static string Serialize(object o)
		{
			return JsonSerializer.Serialize(o, m_options);
		}
		public static async Task SerializeAsync(Stream into, object o)
		{
			await JsonSerializer.SerializeAsync(into, o,  m_options);
		}
		
		public static T Deserialize<T>(string source)
		{
			return JsonSerializer.Deserialize<T>(source, m_options);
		}
		
		public static async Task<T> DeserializeAsync<T>(Stream source)
		{
			return await JsonSerializer.DeserializeAsync<T>(source, m_options);
		}
	}
}