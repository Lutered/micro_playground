using Microsoft.AspNetCore.Http;
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
        public bool IsSuccess { get; set; }
        public T? Value { get; set; }
        public HandlerError Error { get; set; }

        public virtual IActionResult ToActionResult()
        {
            if (this.IsSuccess) return new OkObjectResult(this.Value);

            switch (this.Error.Type)
            {
                case HandlerErrorType.BadRequest:
                case HandlerErrorType.Validation:
                    return new BadRequestObjectResult(this.Error.Message);
                case HandlerErrorType.NotFound: return new NotFoundObjectResult(this.Error.Message);
            }

            var result = new ObjectResult(this.Error.Message);
            result.StatusCode = (int)HttpStatusCode.InternalServerError;
            return result;
        }

        public virtual IResult ToResult()
        {
            if (this.IsSuccess) return Results.Ok(this.Value);

            switch (this.Error.Type)
            {
                case HandlerErrorType.BadRequest:
                case HandlerErrorType.Validation: 
                    return Results.BadRequest(this.Error.Message);
                case HandlerErrorType.NotFound: return Results.NotFound(this.Error.Message);
            }

            return Results.Problem(this.Error.Message, statusCode: 500);
        }
    }
}
