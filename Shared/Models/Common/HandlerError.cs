using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Common
{
    public enum HandlerErrorType
    {
        BadRequest,
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Internal
    }

    public class HandlerError
    {
        public string Message { get; set; }
        public HandlerErrorType? Type { get; set; }

        public HandlerError(string message, HandlerErrorType? type = null)
        {
            Message = message;
            Type = type;
        }
    }
}
