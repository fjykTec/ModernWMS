
using System.Security.Cryptography;
using System.Text;

namespace ModernWMS.Core.Utility
{
    /// <summary>
    /// md5 Helper
    /// </summary>
    public static class Md5Helper
    {
        /// <summary>
        /// 32bit UTF8 MD5 Encrypt
        /// </summary>
        /// <param name="plaintext">明文</param>
        /// <returns></returns>
        public static string Md5Encrypt32(string plaintext)
        {
            string pwd = string.Empty;
            if (!string.IsNullOrEmpty(plaintext) && !string.IsNullOrWhiteSpace(plaintext))
            {
                MD5 md5 = MD5.Create();
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(plaintext));
                foreach (var item in s)
                {
                    pwd = string.Concat(pwd, item.ToString("x2"));
                }
            }
            return pwd;
        }
    }
}
