using System.Collections.Generic;


namespace CardDB.Modules.APIModule.Models.Updates
{
	public class UpdateLogSetModel
	{
		public UpdateLogModel[] updates { get; }
		
		
		public UpdateLogSetModel(IEnumerable<UpdateLog> data)
		{
			var lst = new List<UpdateLogModel>();

			foreach (var log in data)
			{
				lst.Add(new UpdateLogModel(log));
			}
			
			updates = lst.ToArray();
		}
	}
}