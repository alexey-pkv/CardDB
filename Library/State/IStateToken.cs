using System;

namespace Library.State
{
	public interface IStateToken
	{
		public void Complete();
		public void Fail(Exception e);
	}
}