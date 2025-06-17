using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.AuthApi.Requests
{
    public class Register
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
    }
}
