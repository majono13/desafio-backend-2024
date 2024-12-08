using System.Security.Cryptography;
using System.Text;

namespace InovaBank.Application.Services.Cryptography
{
    public class PasswordEncripter
    {
        public PasswordEncripter()
        {
        }

        public string Encrypt(string password)
        {

            password = password.Trim();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = SHA512.HashData(bytes);

            return StringBytes(hashBytes);
        }

        private static string StringBytes(byte[] bytes) //Converte o array de bytes em uma string
        {
            var sb = new StringBuilder();

            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            };
            return sb.ToString();
        }
    }
}
