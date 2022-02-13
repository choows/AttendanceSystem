using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Functions
{
    public class Encryption
    {
        public string CreateMD5Hash(string input)
        {
            input = ModifyString(input);
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
        private string ModifyString(string inpt)
        {
            return inpt;
            /*
            int[] Position = { 1, 2, 5, 8, 10, 12, 15, 19 };
            char[] inptchr = inpt.ToCharArray();
            string TempStr = "aBcDeFgHiJkLmNoPqRsTuVwXyZ";
            foreach(int p in Position)
            {
                if(Position.Length >= p)
                {
                    inptchr[p] = TempStr[p];
                }
            }
            return inptchr.ToString();*/

        }
        public string GetRandomGeneratedString()
        {
            return CreateMD5Hash(RandomString());
        }
        public string RandomStringWithParam(int size = 20 , bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;
            Random _random = new Random();
            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
        private string RandomString()
        {
            int size = 40;
            bool lowerCase = false;
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;
            Random _random = new Random();
            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
