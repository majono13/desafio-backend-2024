using System.Security.Cryptography;
using System.Text;

namespace InovaBank.Application.Services.Cryptography
{
    public class PasswordEncripter
    {
        private readonly string _additionalString;
        public PasswordEncripter(string additionalString)
        {
            _additionalString = additionalString;
        }

        public string Encrypt(string password)
        {
            var newPassword =  $"{password}${_additionalString}";

            var bytes = Encoding.UTF8.GetBytes(newPassword);
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
