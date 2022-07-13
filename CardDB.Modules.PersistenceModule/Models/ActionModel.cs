using System;
using System.Collections.Generic;
using CardDB.Modules.PersistenceModule.Base;
using CardDB.Modules.PersistenceModule.Models.ActionParts;


namespace CardDB.Modules.PersistenceModule.Models
{
	public class ActionModel : IDataModel<Action>
	{
		private Action m_action;
		
		
		private string GetActionTypeValue()
		{
			switch (m_action.ActionType)
			{
				case ActionType.CreateCard:
					return "CreateCard";
				case ActionType.DeleteCard:
					return "DeleteCard";
				case ActionType.ModifyCard:
					return "ModifyCard";
				case ActionType.CreateView:
					return "CreateView";
				case ActionType.ModifyView:
					return "ModifyView";
				case ActionType.DeleteView:
					return "DeleteView";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		private void SetActionTypeValue(string from)
		{
			switch (from)
			{
				case "CreateCard":
					m_action.ActionType = ActionType.CreateCard;
					break;
				case "DeleteCard":
					m_action.ActionType = ActionType.DeleteCard;
					break;
				case "ModifyCard":
					m_action.ActionType = ActionType.ModifyCard;
					break;
				case "CreateView":
					m_action.ActionType = ActionType.CreateView;
					break;
				case "ModifyView":
					m_action.ActionType = ActionType.ModifyView;
					break;
				case "DeleteView":
					m_action.ActionType = ActionType.DeleteView;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		
		public ActionModel() {}
		
		public ActionModel(Action action)
		{
			From(action);
		}


		public string PrimaryID => "ID";
		

		public void From(Action obj)
		{
			m_action = obj;
		}

		public void From(Dictionary<string, object> data)
		{
			m_action = new Action();
			
			if (data.TryGetValue("ID", out var id) && id != null)
			{
				m_action.Sequence = ulong.Parse((string)id);
			}
			
			if (data.TryGetValue("SystemID", out var sysID) && id != null)
			{
				m_action.GeneratedID = (string)sysID;
			}
			
			if (data.TryGetValue("Type", out var t) && t != null)
			{
				SetActionTypeValue((string)t);
			}
			
			if (data.TryGetValue("Data", out var d) && d != null)
			{
				ActionData.SetData(m_action, (string)d);
			}
		}

		public Dictionary<string, object> ToData()
		{
			return new Dictionary<string, object>
			{
				{ "ID",			m_action.Sequence },
				{ "SystemID",	m_action.GeneratedID },
				{ "Type",		GetActionTypeValue() },
				{ "Data",		ActionData.GetData(m_action) }
			};
		}

		public Action ToObject()
		{
			return m_action;
		} 
		
		
		public void SetAutoIncID(long val)
		{
			m_action.Sequence = (ulong)val;
		}
	}
}