using System;
using CardDB;
using NUnit.Framework;

namespace Test.CardDB
{
	public class OrderValueTest
	{
		private int Compare(object[] a, object[] b)
		{
			var valueA = new OrderValue { Value = a };
			var valueB = new OrderValue { Value = b };
			
			return valueA.CompareTo(valueB);
		}
		
		private void AssertCompare(int result, object[] a, object[] b)
		{
			Assert.AreEqual(result, Compare(a, b));
			Assert.AreEqual(result * -1, Compare(b, a));
		}
		
		
		[Test]
		public void EmptyValues_Return0()
		{
			AssertCompare(0, new object[] {}, new object[] {});
		}
		
		[Test]
		public void Strings()
		{
			AssertCompare(0, new object[] { "" }, new object[] { "" });
			AssertCompare(0, new object[] { "hello" }, new object[] { "hello" });
			
			AssertCompare(1, new object[] { "hello 2" }, new object[] { "" });
			AssertCompare(1, new object[] { "hello 2" }, new object[] { "hello" });
			AssertCompare(1, new object[] { "hello 2" }, new object[] { "hello 1" });
			AssertCompare(1, new object[] { "hello Z" }, new object[] { "hello 1" });
			AssertCompare(1, new object[] { "hello z" }, new object[] { "hello Z" });
			AssertCompare(1, new object[] { "hello a" }, new object[] { "hello Z" });
			AssertCompare(1, new object[] { "z" }, new object[] { "abcdefghi" });
		}
		
		[Test]
		public void Ints()
		{
			AssertCompare(0, new object[] { 0 }, new object[] { 0 });
			AssertCompare(0, new object[] { 10 }, new object[] { 10 });
			AssertCompare(0, new object[] { -10 }, new object[] { -10 });
			
			AssertCompare(1, new object[] { 10 }, new object[] { -10 });
			AssertCompare(1, new object[] { 10 }, new object[] { 5 });
			AssertCompare(1, new object[] { -5 }, new object[] { -10 });
		}
		
		[Test]
		public void Doubles()
		{
			AssertCompare(0, new object[] { 0.0 }, new object[] { 0.0 });
			AssertCompare(0, new object[] { 10.0 }, new object[] { 10.0 });
			AssertCompare(0, new object[] { -10.0 }, new object[] { -10.0 });
			AssertCompare(0, new object[] { 0.1 }, new object[] { 0.1 });
			
			AssertCompare(1, new object[] { 10.0 }, new object[] { -10.0 });
			AssertCompare(1, new object[] { 10.0 }, new object[] { 5.0 });
			AssertCompare(1, new object[] { -5.0 }, new object[] { -10.0 });
			
			AssertCompare(1, new object[] { -0.1 }, new object[] { -0.2 });
			AssertCompare(1, new object[] { 0.2 }, new object[] { 0.1 });
			AssertCompare(1, new object[] { -0.01 }, new object[] { -0.99 });
			AssertCompare(1, new object[] { 0.99 }, new object[] { 0.01 });
		}
		
		[Test]
		public void Bools()
		{
			AssertCompare(0, new object[] { true }, new object[] { true });
			AssertCompare(0, new object[] { false }, new object[] { false });
			AssertCompare(1, new object[] { true }, new object[] { false });
		}
		
		[Test]
		public void Nulls()
		{
			AssertCompare(0, new object[] { null }, new object[] { null });
			
			AssertCompare(1, new object[] { true }, new object[] { null });
			AssertCompare(1, new object[] { false }, new object[] { null });
			
			AssertCompare(1, new object[] { -1 }, new object[] { null });
			AssertCompare(1, new object[] { 0 }, new object[] { null });
			AssertCompare(1, new object[] { 1 }, new object[] { null });
			
			AssertCompare(1, new object[] { 0.0 }, new object[] { null });
			AssertCompare(1, new object[] { -0.1 }, new object[] { null });
			AssertCompare(1, new object[] { 0.1 }, new object[] { null });
			
			AssertCompare(1, new object[] { "" }, new object[] { null });
			AssertCompare(1, new object[] { "hello world" }, new object[] { null });
			AssertCompare(1, new object[] { "null" }, new object[] { null });
		}
	}
}