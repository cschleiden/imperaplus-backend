using System.Diagnostics;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.DTO;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Fluent;

namespace ImperaPlus.Web.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var domainException = context.Exception as DomainException;
            if (domainException != null)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new ErrorResponse(domainException.ErrorCode.ToString(), domainException.Message));                
                return;
            }

            var applicationException = context.Exception as ApplicationException;
            if (applicationException != null)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new ErrorResponse(applicationException.ErrorCode.ToString(), applicationException.Message));               
                return;
            }

            // Exception could not be handled, should not happen
            if (context.Exception != null)
            {
#if DEBUG
                Debugger.Launch();
#endif

                new TelemetryClient().TrackException(context.Exception);

                // Log exception
                Log.Fatal().Exception(context.Exception).Write();
            }
        }
    }
}
