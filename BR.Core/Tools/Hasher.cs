using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BR.Core.Tools
{
    public static class Hasher
    {
        public static string GetHash(string input)
        {
            using (var md5 = MD5.Create())
            {
                md5.Initialize();

                md5.ComputeHash(Encoding.UTF8.GetBytes(input));

                return string.Join("", Convert.ToBase64String(md5.Hash).Take(15));
            }
        }
    }
}
