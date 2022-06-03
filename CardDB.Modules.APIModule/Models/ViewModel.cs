using System.Collections.Generic;


namespace CardDB.Modules.APIModule.Models
{
	public class ViewModel
	{
		public string id { get; }
		
		
		public ViewModel(View v)
		{
			id = v.ID;
		}
	}
}