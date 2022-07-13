using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CardDB.Conditions;
using CardDB.Conditions.FieldConditions;


namespace CardDB.Modules.PersistenceModule.Models.ActionParts
{
	public class ConditionData
	{
		private const string FIELD_EXISTS	= "field_exists";
		private const string IS_VALUE		= "is_value";
		private const string REGEX			= "regex";
		private const string AND			= "and";
		private const string OR				= "or";
		private const string NOT			= "not";
		
		
		private string type	{ get; set; }
		
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string field { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string value { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public ConditionData[] children { get; set; }
		
		
		
		private void SetChildren(IEnumerable<ICondition> source)
		{
			children = source
				.Select(c => new ConditionData(c))
				.ToArray();
		}
		
		private List<ICondition> GetChildren()
		{
			return children
				.Select((c) => c.Create())
				.ToList();
		}
		
		private ICondition GetChild()
		{
			return children
				.Select((c) => c.Create())
				.First();
		}
		
		
		
		public ConditionData() {}
		public ConditionData(ICondition condition)
		{
			if (condition is FieldExistsCondition)
			{
				var c = (FieldExistsCondition)condition;
				
				type = FIELD_EXISTS;
				field = c.Field;
			}
			else if (condition is RegexCondition)
			{
				var c = (RegexCondition)condition;
				
				type = FIELD_EXISTS;
				field = c.Field;
				value = c.Regex.ToString();
			}
			else if (condition is AndCondition)
			{
				var c = (AndCondition)condition;
				
				type = AND;
				SetChildren(c.Children);
			}
			else if (condition is OrCondition)
			{
				var c = (OrCondition)condition;
				
				type = OR;
				SetChildren(c.Children);
			}
			else if (condition is NotCondition)
			{
				var c = (NotCondition)condition;
				
				type = OR;
				SetChildren(new []{ c.Child });
			}
			else
			{
				throw new InvalidOperationException($"Unhandled type {condition.GetType()}");
			}
		}
		
		
		public ICondition Create()
		{
			switch (type)
			{
				case FIELD_EXISTS:
					return new FieldExistsCondition { Field = field };
				
				case IS_VALUE:
					return new IsValueCondition { Field = field, Value = value };
				
				case REGEX:
					return new RegexCondition { Field = field, Regex = new Regex(value) };
				
				case AND:
					return new AndCondition { Children = GetChildren() };
				
				case OR:
					return new OrCondition { Children = GetChildren() };
				
				case NOT:
					return new NotCondition { Child = GetChild() };
				
				default:
					throw new Exception($"Unknown type {type}");
			}
		}
	}
}