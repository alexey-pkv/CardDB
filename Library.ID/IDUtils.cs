namespace Library.ID
{
	public class IDUtils
	{
		public static string ToID(ulong i)
		{
			return BaseConverter.Convert(i, 36).PadLeft(12, '0');
		}
	}
}