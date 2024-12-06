﻿using System;
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
                context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception?.ErrorMessages));
            }
        }

        private void ThrowUnknowException(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //context.Result = new ObjectResult(new ResponseErrorJson(ErrorsMessages.UNKNOW_ERRROR));
        }
    }
}