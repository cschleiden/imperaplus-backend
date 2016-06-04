using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Web.Http.Filters;

namespace ImperaPlus.Backend.Diagnostics
{
    public class MiniProfilerActionFilter : ActionFilterAttribute
    {
        private const string StackKey = "ProfilingActionFilterStack";

        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            MiniProfiler.Start();

            if (MiniProfiler.Current != null)
            {
                Stack<IDisposable> stack;
                if (actionContext.Request.Properties.ContainsKey(StackKey))
                {
                    stack = actionContext.Request.Properties[StackKey] as Stack<IDisposable>;
                }
                else
                {
                    stack = new Stack<IDisposable>();
                    actionContext.Request.Properties[StackKey] = (object)stack;
                }
                MiniProfiler current = MiniProfiler.Current;
                if (current != null)
                {
                    string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    string actionName = actionContext.ActionDescriptor.ActionName;
                    stack.Push(MiniProfilerExtensions.Step(current, "Controller: " + controllerName + actionName));
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Request.Properties.ContainsKey(StackKey))
            {
                var stack = actionExecutedContext.Request.Properties[StackKey] as Stack<IDisposable>;
                if (stack != null && stack.Count > 0)
                {
                    stack.Pop().Dispose();
                }
            }

            MiniProfiler.Stop();
        }
    }
}