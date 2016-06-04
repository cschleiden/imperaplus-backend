using NLog.Fluent;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace ImperaPlus.Backend
{
    public class OopsExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            context.Result = new TextPlainErrorResult
            {
                Request = context.ExceptionContext.Request,
                Content = "Oops! Sorry! Something went wrong. Please contact info@imperaonline.de so we can try to fix it."
            };

            Log.Error().Message("Exception occured {0}", context.Exception).Exception(context.Exception).Write();

            base.Handle(context);
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public string Content { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(this.Content),
                    RequestMessage = this.Request
                };

                return Task.FromResult(response);
            }
        }
    }
}