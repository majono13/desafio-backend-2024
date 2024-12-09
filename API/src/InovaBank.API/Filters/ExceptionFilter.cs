using System;
using System.Net;
using InovaBank.Application.Exceptions.ExceptionsBase;
using InovaBank.Communication.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InovaBank.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
           if (context.Exception is InovaBankException)
            {
                HandleProjectException(context);
            }
           else
            {
                ThrowUnknowException(context);
            }
        }

        private void HandleProjectException(ExceptionContext context)
        {
            if (context.Exception is ErrorOnValidationException)
            {
                var exception = context.Exception as ErrorOnValidationException;

                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception?.ErrorMessages!));
            }
            else if (context.Exception is InvalidLoginException)
            {
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
                }
            }
            else if (context.Exception is ReceitaWSException)
            {
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
                }
            }
            else if (context.Exception is AccountNotFoundException)
            {
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));
                }
            }

            else if (context.Exception is InvalidTokenException)
            {
                {
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
                }
            }
        }

        private void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
           // context.Result = new ObjectResult(new ResponseErrorJson(ErrorsMessages.UNKNOW_ERRROR));
        }
    }
}
