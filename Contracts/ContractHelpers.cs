using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public static class ContractHelpers
    {
        public static T ReturnError<T>(int code, string message) where T : BaseResponseContract
        {
            return (T)new BaseResponseContract
            {
                IsError = true,
                StatusCode = code,
                ErrorMessage = message
            };
        }
    }
}
