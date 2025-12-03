using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Common
{
    public abstract class BaseResult<T>
    {
        public bool IsSuccess { get; set;  }
        public T? Value { get; set; }
        public HandlerError Error { get; set; }

        public virtual IActionResult ToActionResult()
        {
            if(this.IsSuccess) return new OkObjectResult(this.Value);

            switch(this.Error.Type)
            {
                case HandlerErrorType.NotFound: return new NotFoundObjectResult(this.Value);
                case HandlerErrorType.Validation: return new BadRequestObjectResult(this.Value);
            }

            var result = new ObjectResult(this.Value);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            return result;
        }
    }
}
