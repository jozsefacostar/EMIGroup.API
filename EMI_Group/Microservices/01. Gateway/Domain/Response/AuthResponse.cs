using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Responses
{
    public class AuthResponse
    {
        public string UserName { get; set; }
        public string JWToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
