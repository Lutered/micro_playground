using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.AuthApi.Responses
{
    public class UserResponse : BaseResponseContract
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public  string Username { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
