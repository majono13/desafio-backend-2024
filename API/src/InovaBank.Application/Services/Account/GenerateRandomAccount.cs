namespace InovaBank.Application.Services.Account
{
    public class GenerateRandomAccount
    {
        public string GenerateRandomValue()
        {
            Random random = new Random();
            return random.Next(10000000, 100000000).ToString();
        }

        public string GenerateRandomDigit() 
        {
            Random random = new Random();
            return random.Next(0, 10).ToString();
        }
    }
}
