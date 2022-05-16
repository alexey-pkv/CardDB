using System;
using System.Threading;
using System.Security.Cryptography;
using System.Collections.Generic;


namespace Library.ID
{
	public static class IDGenerator
	{
		private const ulong RAND_MASK	= 0b00111111_11111111_11111111_11111111_00000000_00000000_00000000_00000000;
		private const ulong MASK_TIME	= 0b00000000_00000000_00000000_00000000_11111111_11111111_11111111_11111111;
		
		
		private static ulong m_lastSecond;
		private static HashSet<ulong> m_set = new();
		private static RNGCryptoServiceProvider m_generator = new();
		
		
		private static ulong GenerateSuffix()
		{
			byte[] data = new byte[sizeof(ulong)];
			m_generator.GetBytes(data);
			return BitConverter.ToUInt64(data, 0) & RAND_MASK;
		}
		
		
		private static ulong GetRandomValue(ulong time)
		{
			var rand = GenerateSuffix();
			
			if (m_lastSecond != time)
			{
				m_lastSecond = time;
				m_set.Clear();
			}
			else
			{
				while (m_set.Contains(rand))
				{
					rand = GenerateSuffix();
				}
			}
			
			m_set.Add(rand);
			
			return rand;
		}
		
		
		
		public static string Generate()
		{
			ulong val;
			ulong time;
			
			while (true)
			{
				time = DateTime.Now.UnixTimestamp() & MASK_TIME;
				
				lock (m_set)
				{
					if (m_lastSecond != time || m_set.Count < 1000000)
					{
						val = GetRandomValue(time);
						break;
					}
				}
				
				Thread.Sleep(100);
			}
			
			val = val | time;
			
			return BaseConverter.Convert(val, 36);
		}
	}
}