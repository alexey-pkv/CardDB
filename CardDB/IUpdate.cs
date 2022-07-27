using System;


namespace CardDB
{
	public interface IUpdate
	{
		public Bucket Bucket { get; init; }
		public ulong Sequence { get; init; }
		public UpdateTarget TargetType { get; }
		public UpdateType UpdateType { get; init; }
	}
}