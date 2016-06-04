using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using ImperaPlus.Application.Exceptions;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.DTO;
using NLog.Fluent;
using System.Diagnostics;

namespace ImperaPlus.Backend.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var domainException = context.Exception as DomainException;
            if (domainException != null)
            {
                throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.BadRequest,
                    new ErrorResponse(domainException.ErrorCode.ToString(), domainException.Message)));
            }

            var applicationException = context.Exception as ApplicationException;
            if (applicationException != null)
            {
                throw new HttpResponseException(context.Request.CreateResponse(HttpStatusCode.BadRequest,
                    new ErrorResponse(applicationException.ErrorCode.ToString(), applicationException.Message)));
            }

            // Exception could not be handled, should not happen
            if (context.Exception != null)
            {
#if DEBUG
                Debugger.Launch();
#endif

                Log.Fatal().Message(context.Exception.ToString()).Write();

                // Log exception
                Log.Fatal().Exception(context.Exception).Write();
            }

            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
} 