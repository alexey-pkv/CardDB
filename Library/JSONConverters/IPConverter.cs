using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Library.JSONConverters
{
	public class IPConverter : JsonConverter<IPAddress>
	{
		public override IPAddress Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			var str = reader.GetString();
			
			if (str == null)
				return null;
			
			return IPAddress.Parse(str);
		}

		public override void Write(
			Utf8JsonWriter writer,
			IPAddress value,
			JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.ToString());
		}
	}
}