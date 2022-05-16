using System;


namespace CardDB
{
	public interface IUpdate : IDBEntity
	{
		public Int64 Sequence { get; set; }
		public UpdateTarget TargetType { get; set; }
	}
}