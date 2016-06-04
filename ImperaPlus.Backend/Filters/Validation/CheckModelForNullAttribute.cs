using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ImperaPlus.Backend.Validation
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CheckModelForNullAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate;

        public CheckModelForNullAttribute()
            : this(arguments =>
                arguments.ContainsValue(null))
        { }

        public CheckModelForNullAttribute(Func<Dictionary<string, object>, bool> checkCondition)
        {
            this._validate = checkCondition;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (this._validate(actionContext.ActionArguments))
            {
                var message = string.Join(", ",
                    actionContext.ActionArguments.Where(x => x.Value == null).Select(x => x.Key).Select(
                        x => string.Format(CultureInfo.InvariantCulture, "{0} cannot be null", x)));

                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, message);
            }
        }
    }
}