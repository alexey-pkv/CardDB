using System;

namespace Library
{
	public static class DateTimeExtension
	{
		private static readonly DateTime ORIGIN = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		
		
		public static ulong UnixTimestamp(this DateTime time)
		{
			return (ulong)(time.ToUniversalTime() - ORIGIN).TotalSeconds;
		}
	}
}