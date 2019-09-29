using System;
using System.Security.Cryptography;
using System.Text;

namespace Project_Email_MVC.Models
{
    public class HashPassword
    {
        public string GetMd5Hash(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public bool VerifyMd5Hash(string password, string hash)
        {
            string hashOfInput = GetMd5Hash(password);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}