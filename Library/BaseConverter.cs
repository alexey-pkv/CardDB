using System;
using System.Text;


namespace Library
{
    public static class BaseConverter
    {
        private const string CHARS = "0123456789abcdefghijklmnopqrstuvwxyz";

        
        private static string ConvertToBase(ulong b10, ulong targetBase)
        {
            if (targetBase < 2)
            {
                throw new ArgumentException("Target base must be greater than 2.", nameof(targetBase));
            }

            if (targetBase > 36)
            {
                throw new ArgumentException("Target base must be less than 36.", nameof(targetBase));
            }

            if (targetBase == 10)
            {
                return b10.ToString();
            }

            StringBuilder result = new StringBuilder();

            while (b10 >= targetBase) 
            {
                var mod = b10 % targetBase;
                result.Append(CHARS[(int)mod]);
                b10 /= targetBase;
            }

            result.Append(CHARS[(int)b10]);

            return Reverse(result.ToString());
        }
        
        private static ulong GetValue(char of)
        {
            if (of <= '9' && of >= '0')
            {
                return (ulong)(of - '0'); 
            }
            else if ('a' <= of && of <= 'z')
            {
                return (ulong)(of - 'a') + 10;
            }
            else
            {
                 throw new ArgumentException("Bad input", nameof(of));
            }
        }

        private static ulong ConvertFromBase(string value, int fromBase)
        {
            if (fromBase < 2)
            {
                throw new ArgumentException("Base must be greater than 2.", nameof(fromBase));
            }

            if (fromBase > 36)
            {
                throw new ArgumentException("Base must be less than 36.", nameof(fromBase));
            }

            if (fromBase == 10)
            {
                return ulong.Parse(value);
            }
            
            ulong factor = (ulong)fromBase;
            ulong scale = 1;
            ulong result = 0;
            
            for (int i = value.Length - 1; i >= 0; i--)
            {
                result += scale * GetValue(value[i]);
                scale *= factor;
            }
            
            return result;
        }

        private static string Reverse(string s)
        {
            char[] charArray = new char[s.Length];
            var len = s.Length - 1;
            for (var i = 0; i <= len; i++)
            {
                charArray[i] = s[len - i];
            }
            
            return new string(charArray);
        }
        
        
        public static string Convert(string str, int fromBase, int toBase)
        {
            return ConvertToBase(ConvertFromBase(str, fromBase), (ulong)toBase);
        }
        
        public static string Convert(ulong num, ulong to = 36)
        {
            return ConvertToBase(num, to);
        }
    }
}