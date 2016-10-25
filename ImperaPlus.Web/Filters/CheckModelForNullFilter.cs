using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ImperaPlus.Web.Filters
{
    public class CheckModelForNull : IActionFilter
    {
        private readonly Func<IDictionary<string, object>, bool> _validate;

        public CheckModelForNull()
            : this(arguments => arguments.Any(kvp => kvp.Value == null))
        { }

        public CheckModelForNull(Func<IDictionary<string, object>, bool> checkCondition)
        {
            this._validate = checkCondition;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (this._validate(actionContext.ActionArguments))
            {
                var message = string.Join(", ",
                    actionContext.ActionArguments.Where(x => x.Value == null).Select(x => x.Key).Select(
                        x => string.Format(CultureInfo.InvariantCulture, "{0} cannot be null", x)));

                actionContext.Result = new BadRequestObjectResult(message);
            }
        }
    }
}
