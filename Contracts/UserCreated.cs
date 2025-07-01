using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public class UserCreated
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
