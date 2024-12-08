using Microsoft.AspNetCore.Http;

namespace InovaBank.Application.Services.Account
{
    public class ConvertFileToBase64
    {
        public async Task<string> ConvertToString(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }
}
