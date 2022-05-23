using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Library.JSONConverters
{
	public class DateTimeConverter : JsonConverter<DateTime>
	{
		private const string FORMAT = "yyyy-MM-dd HH:mm:ss";
		private const string ZERO = "0000-00-00 00:00:00";
		
		
		public override DateTime Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			var value = reader.GetString();
			
			if (value == ZERO)
			{
				return DateTime.MinValue;
			}
			else
			{
				return DateTime.ParseExact(value, FORMAT, CultureInfo.InvariantCulture);
			}
		}
		
		public override void Write(
			Utf8JsonWriter writer,
			DateTime value,
			JsonSerializerOptions options)
		{
			if (value == DateTime.MinValue)
			{
				writer.WriteStringValue(ZERO);
			}
			else
			{
				writer.WriteStringValue(value.ToString(FORMAT, CultureInfo.InvariantCulture));
			}
		}
	}
}