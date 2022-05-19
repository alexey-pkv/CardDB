using System;


namespace CardDB
{
	public interface IUpdate
	{
		public Int64 Sequence { get; set; }
		public UpdateTarget TargetType { get; }
	}
}