using System.Text.RegularExpressions;

namespace InovaBank.Application.Services.Account
{
    public class ReplaceCnpj
    {
        public string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9\s]", "").Trim();
        }
    }
}
