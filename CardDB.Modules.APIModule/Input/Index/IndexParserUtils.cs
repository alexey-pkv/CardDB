using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;
using CardDB.Modules.APIModule.Input.Exceptions;

namespace CardDB.Modules.APIModule.Input.Index
{
	public static class IndexParserUtils
	{
		public static ICondition ParseRecursive(JsonElement element, string path)
		{
			var parser = TypeParserFactory.GetType(element, path);
			return parser.Parse(element, path);
		}
		
		public static void ValidateJsonElementType(JsonElement root, JsonValueKind expected, string name, string path)
		{
			if (root.ValueKind != expected)
			{
				throw new InvalidConditionSetupException(
					$"Expecting {name} at {path} but got {root.ValueKind} instead", path);
			}
		}
		
		public static void ValidateArray(JsonElement root, string path)
		{
			ValidateJsonElementType(root, JsonValueKind.Array, "array", path);
		}
		
		public static void ValidateString(JsonElement root, string path)
		{
			ValidateJsonElementType(root, JsonValueKind.String, "string", path);
		}
		
		public static void ValidateObject(JsonElement root, string path)
		{
			ValidateJsonElementType(root, JsonValueKind.Object, "object", path);
		}
		
		public static JsonElement GetField(JsonElement root, string field, string path)
		{
			if (!root.TryGetProperty(field, out var element))
			{
				throw new InvalidConditionSetupException($"Missing property {field}", path);
			}
			
			return element;
		}
		
		
		public static ICondition ParseConditionInField(JsonElement root, string field, string path)
		{
			ValidateObject(root, path);
			
			var conditionElement = GetField(root, field, path);
			
			return ParseRecursive(conditionElement, $"{path}.{field}");
		}
		
		public static string ParseStringField(JsonElement root, string field, string path)
		{
			var strField = GetField(root, field, path);
			ValidateString(strField, $"{path}.{field}");
			return strField.GetString();
		}
		
		public static Regex ParseRegexField(JsonElement root, string field, string path)
		{
			var strField = GetField(root, field, path);
			ValidateString(strField, $"{path}.{field}");
			
			var regex = strField.GetString();

			try
			{
				return new Regex(regex ?? throw new InvalidOperationException());
			}
			catch (RegexParseException e)
			{
				throw new InvalidConditionSetupException($"Invalid regex `regex` in {field}", path);
			}
		}
		
		public static List<ICondition> ParseConditionsInField(JsonElement root, string field, string path)
		{
			ValidateObject(root, path);
			
			var fieldPath = $"{path}.{field}";
			var conditionElements = GetField(root, field, path);
			
			ValidateArray(conditionElements, fieldPath);
			
			var length = conditionElements.GetArrayLength();
			var list = new List<ICondition>(length);
			
			for (var i = 0; i < length; i++)
			{
				list.Add(ParseRecursive(conditionElements[i], $"{fieldPath}.{i}"));
			}
			
			return list;
		}
	}
}