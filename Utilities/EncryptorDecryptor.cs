using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVersion2.Utilities
{
    public class EncryptorDecryptor
    {
        public string MD5Hash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                sb.Append(b);
            }
            return sb.ToString();
        }

        public bool VerifyHash(string input, string hash)
        {
            // Hash the input
            string hashOfInput = MD5Hash(input);
            // Create a StringComparer an compare the hashes
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (0 == comparer.Compare(hashOfInput, hash));
        }

        
        
        }
}
