using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ATestApplication.Models;
using ATestApplication.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ATestApplication.Controllers
{
    [Route("api/[controller]")]
    public class BaseController: ControllerBase
    {
        protected ActionResult HandleException<T>(Exception exception, string message = null) where T : class
        {
            var respMessage = message;
            if (string.IsNullOrWhiteSpace(respMessage))
            {
                respMessage = exception?.Message;
            }
            return exception switch
            {
                EntityNotFoundException _ => NotFound(GetResponseResult<T>(null, new NotFoundObjectResult(respMessage))),
                ConflictException _ => Conflict(GetResponseResult<T>(null, new ConflictObjectResult(respMessage))),
                FailedDependencyException _ => StatusCode(StatusCodes.Status424FailedDependency, GetResponseResult<T>(null, new BadRequestObjectResult(respMessage))),
                BadRequestException _ => BadRequest(GetResponseResult<T>(null, new BadRequestObjectResult(respMessage))),
                UnexpectedException _ => UnprocessableEntity(GetResponseResult<T>(null, new UnprocessableEntityObjectResult(respMessage))),
                _ => UnprocessableEntity(GetResponseResult<T>(null, new UnprocessableEntityObjectResult(string.IsNullOrWhiteSpace(message) ? "Unknown error occurred" : message))),
            };

            // TODO Handle exception
        }

        protected ResponseModel<T> GetResponseResult<T>(T data, ObjectResult objectResult = null)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                var responseText = string.Join(" ", errors);
                return new ErrorResponseModel<T>
                {
                    Error = objectResult != null ? new ErrorModel
                    {
                        Message = responseText
                    } : null,
                    Data = data ?? default
                };
            }
            else
            {
                ErrorModel error = null;
                if (!(objectResult is CreatedResult) && objectResult != null)
                {
                    error = new ErrorModel
                    {
                        Message = objectResult.Value.ToString()
                    };
                }
                if (error != null)
                {
                    return new ErrorResponseModel<T>
                    {
                        Error = error,
                        Data = data ?? default
                    };
                }
                else
                {
                    return new SuccessResponseModel<T>
                    {
                        Data = data ?? default
                    };
                }
            }
        }
    }
}
