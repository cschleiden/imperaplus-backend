using System.Diagnostics;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog.Fluent;

namespace ImperaPlus.Web.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is DomainException domainException)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new ErrorResponse(domainException.ErrorCode.ToString(),
                    domainException.Message));

                base.OnException(context);

                return;
            }

            if (context.Exception is ApplicationException applicationException)
            {
                context.ExceptionHandled = true;
                context.Result = new BadRequestObjectResult(new ErrorResponse(applicationException.ErrorCode.ToString(),
                    applicationException.Message));

                base.OnException(context);

                return;
            }

            // Exception could not be handled, should not happen
            if (context.Exception != null)
            {
#if DEBUG
                Debugger.Launch();
#endif

                // Log exception
                Log.Fatal().Exception(context.Exception).Write();
            }

            base.OnException(context);
        }
    }
}
