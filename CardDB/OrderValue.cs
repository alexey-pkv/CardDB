using System;
using System.Collections;
using System.Collections.Generic;
using CardDB.Exceptions;


namespace CardDB
{
	/// <summary>
	/// Supported types are:
	/// string, int, bool, double, null
	/// </summary>
	public class OrderValue : IComparable<OrderValue>
	{
		public object[] Value { get; init; }
		
		
		private static int CompareNull(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			else if (a == null)
			{
				return -1;
			}
			else
			{
				return 1;
			}
		}
		
		private static int CompareWithString(string a, object b)
		{
			return String.Compare(a, b.ToString(), StringComparison.Ordinal);
		}
		
		private static int CompareWithInt(int a, object b)
		{
			if (b is int)
			{
				return a.CompareTo((int)b);
			}
			else if (b is bool)
			{
				return a.CompareTo((bool)b ? 1 : 0);
			}
			else if (b is double)
			{
				return ((double)a).CompareTo((double)b);
			}
			else
			{
				throw new FatalCardDBException("Unexpected type: " + b.GetType().FullName);
			}
		}
		
		private static int CompareWithDouble(double a, object b)
		{
			if (b is double)
			{
				return a.CompareTo((double)b);
			}
			else if (b is bool)
			{
				return a.CompareTo((bool)b ? 1.0 : 0.0);
			}
			else
			{
				throw new FatalCardDBException("Unexpected type: " + b.GetType().FullName);
			}
		}
		
		private static int CompareValues(object a, object b)
		{
			if (a == null || b == null)
			{
				return CompareNull(a, b); 
			}
			else if (a is string)
			{
				return CompareWithString((string)a, b);
			}
			else if (b is string)
			{
				return CompareWithString((string)b, a) * -1;
			}
			else if (a is int)
			{
				return CompareWithInt((int)a, b);
			}
			else if (b is int)
			{
				return CompareWithInt((int)b, a) * -1;
			}
			else if (a is double)
			{
				return CompareWithDouble((double)a, b);
			}
			else if (b is double)
			{
				return CompareWithDouble((double)b, a) * -1;
			}
			else if (a is bool && b is bool)
			{
				return ((bool)a).CompareTo((bool)b);
			}
			else
			{
				throw new FatalCardDBException(
					"Unexpected types: " + a.GetType().FullName + " and " + b.GetType().FullName);
			}
		}
		
		
		public int CompareTo(OrderValue other)
		{
			for (var i = 0; i < Value.Length; i++)
			{
				if (other.Value.Length <= i)
					return 1;
				
				var result = CompareValues(Value[i], other.Value[i]);
				
				if (result != 0)
				{
					return (result <= -1 ? -1 : 1);
				}
			}
			
			return 0;
		}

		public override int GetHashCode()
		{
			int code = 0;
			
			if (Value.Length != 0)
			{
				foreach (var o in Value)
				{
					code ^= o.GetHashCode();
				}
			}
			
			return code;
		}
	}
}