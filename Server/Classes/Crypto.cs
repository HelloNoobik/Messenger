using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Server.Classes
{
    public class Crypto
    {
        public static string Hash(string pass) 
        {
            using (var hashAlg = SHA1.Create())
            {
                byte[] input = Encoding.UTF8.GetBytes(pass);
                byte[] hash = hashAlg.ComputeHash(input);
                StringBuilder builder = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    builder.Append(b.ToString("X2"));
                }

                return builder.ToString();
            }
        }
    }
}
