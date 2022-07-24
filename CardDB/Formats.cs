using System.Text.RegularExpressions;
using Library;

namespace CardDB
{
	public static class Formats
	{
		private static readonly Regex BUCKET_NAME_FORMAT = new Regex("[a-z0-9_\\-\\.\\:]{8,32}"); 
		
		
		public static string SanitizeBucketName(string name)
		{
			if (!TrySanitizeBucketName(name, out name))
				throw new CardDBException($"Invalid bucket name {name}");
			
			return name;
		}
		
		public static bool TrySanitizeBucketName(string name, out string result)
		{
			result = name.Trim().ToLower();
			
			if (!BUCKET_NAME_FORMAT.IsMatch(name))
				return false;
			
			return true;
		}
	}
}