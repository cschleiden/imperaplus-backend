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
            var client = new TelemetryClient();

            var domainException = context.Exception as DomainException;
            if (domainException != null)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new ErrorResponse(domainException.ErrorCode.ToString(), domainException.Message));

                client.TrackException(domainException);

                return;
            }

            var applicationException = context.Exception as ApplicationException;
            if (applicationException != null)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new ErrorResponse(applicationException.ErrorCode.ToString(), applicationException.Message));

                client.TrackException(applicationException);

                return;
            }

            // Exception could not be handled, should not happen
            if (context.Exception != null)
            {
#if DEBUG
                Debugger.Launch();
#endif

                client.TrackException(context.Exception);

                // Log exception
                Log.Fatal().Exception(context.Exception).Write();
            }
        }
    }
}
