using System;


namespace CardDB
{
	public interface IUpdate
	{
		public ulong Sequence { get; init; }
		public UpdateTarget TargetType { get; }
		public UpdateType UpdateType { get; init; }
	}
}