using System.Security.Cryptography;
using System.Text;

namespace WebHamburgueseria.Utils
{
    public static class Util
    {
        public static string Encrypt(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            input = input.Trim();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();
                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }
    }
}