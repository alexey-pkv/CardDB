using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

using CardDB.Conditions;
using CardDB.Conditions.FieldConditions;


namespace CardDB.Modules.PersistenceModule.Models.CardParts
{
	public class ConditionModel
	{
		private ICondition m_condition;
		
		
		public string type { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<ConditionModel> conditions { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string field { get; set; }
		
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string value { get; set; }
		
		
		private void SetConditions(List<ICondition> cs)
		{
			conditions = new List<ConditionModel>(cs.Count);

			foreach (var c in cs)
			{
				conditions.Add(new ConditionModel(c));
			}
		}
		
		private List<ICondition> GetConditions()
		{
			var lst = new List<ICondition>(conditions.Count);

			foreach (var cm in conditions)
			{
				lst.Add(cm.Get());
			}
			
			return lst;
		}
		
		
		public ConditionModel() {}
		public ConditionModel(ICondition condition)
		{
			m_condition = condition;

			if (condition is NotCondition)
			{
				type = "not";
			}
			else if (condition is FieldExistsCondition fec)
			{
				type = "exists";
				field = fec.Field;
			}
			else if (condition is IsValueCondition ivc)
			{
				type = "value";
				field = ivc.Field;
				value = ivc.Value;
			}
			else if (condition is RegexCondition rc)
			{
				type = "regex";
				field = rc.Field;
				value = rc.Regex.ToString();
			}
			else if (condition is AndCondition ac)
			{
				type = "and";
				SetConditions(ac.Children);
			}
			else if (condition is OrCondition oc)
			{
				type = "or";
				SetConditions(oc.Children);
			}
			else
			{
				throw new NotImplementedException($"Type is not supported - {condition.GetType()}");
			}
		}
		
		
		public ICondition Get()
		{
			if (m_condition == null)
			{
				switch (type)
				{
					case "not":
						m_condition = new NotCondition();
						break;
					
					case "exists":
						m_condition = new FieldExistsCondition { Field = field };
						break;
					
					case "value":
						m_condition = new IsValueCondition { Field = field, Value = value };
						break;
					
					case "regex":
						m_condition = new RegexCondition { Field = field, Regex = new Regex(value) };
						break;
					
					case "and":
						m_condition = new AndCondition { Children = GetConditions() };
						break;
						
					case "or":
						m_condition = new OrCondition { Children = GetConditions() };
						break;
					
					default:
						throw new NotImplementedException($"Type is not supported {type}");
				}
			}
			
			return m_condition;
		}
	}
}