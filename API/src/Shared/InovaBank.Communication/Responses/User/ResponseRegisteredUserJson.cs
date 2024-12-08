using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InovaBank.Communication.Responses.User
{
    public class ResponseRegisteredUserJson
    {
        public string Email { get; set; } = "";
        public string Token { get; set; } = default!;

    }
}
