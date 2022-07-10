using System;
using System.Collections.Generic;
using System.Text.Json;
using Library;


namespace CardDB
{
	public class CardIndex : IComparable<CardIndex>
	{
		public string ID { get; set; }
		public Card Card { get; init; }
		public View View { get; init; }
		public OrderValue Order { get; init; }
		
		public string CardID => Card?.ID ?? ID;
		public string ViewID => View.ID;
		
		
		public int CompareTo(CardIndex other)
		{
			if (ReferenceEquals(this, other)) return 0;
			if (ReferenceEquals(null, other)) return 1;
			
			var result = Order.CompareTo(other.Order);
			
			if (result == 0)
			{
				result = String.Compare(CardID, other.CardID, StringComparison.Ordinal);
				
				if (result < -1) result = -1;
				else if (result > 1) result = 1;
			}
			
			return result;
		}

		public override int GetHashCode()
		{
			return Order.GetHashCode() ^ Card.GetHashCode();
		}
		
		
		public static CardIndex FromJSON(string json)
		{
			CardIndex index;
			List<object> data = null;
			
			try
			{
				data = JSON.Deserialize<List<object>>(json);
			}
			catch (JsonException)
			{
				return null;
			}

			if (data.Count < 1 || data.Count > 100)
				return null;

			for (var i = 0; i < data.Count; i++)
			{
				var item = data[i];
				
				if (item != null && 
				    item is not JsonElement)
				{
					return null;
				}
				else if (item == null)
				{
					continue;
				}
				
				var je = (JsonElement)item;
				
				switch (je.ValueKind)
				{
					case JsonValueKind.String:
						var val = je.GetString();
						
						if (val == null || val.Length > 1024 * 1024)
							return null;
						
						data[i] = val;
						break;
					
					case JsonValueKind.False:
						data[i] = false;
						break;
					
					case JsonValueKind.Null:
						data[i] = null;
						break;
					
					case JsonValueKind.True:
						data[i] = true;
						break;
					
					case JsonValueKind.Number:
						if (!je.TryGetDecimal(out var dec))
							return null;
						
						if (Math.Truncate(dec) == dec)
							data[i] = (int)dec;
						else
							data[i] = (double)dec;
						
						break;
					
					default:
						return null;
				}
			}
			
			var last = data[^1];
			
			if (!(last is string))
				return null;
			
			index = new CardIndex()
			{
				ID = last.ToString(),
				Order = new OrderValue(data.GetRange(0, data.Count - 1))
			};
			
			return index;
		}
	}
}