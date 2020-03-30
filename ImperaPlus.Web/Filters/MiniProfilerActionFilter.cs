using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using StackExchange.Profiling;

namespace ImperaPlus.Web.Filters
{
    public class MiniProfilerActionFilter : IActionFilter
    {
        private const string StackKey = "ProfilingActionFilterStack";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            MiniProfiler.StartNew();

            if (MiniProfiler.Current != null)
            {
                Stack<IDisposable> stack;
                
                if (context.ActionDescriptor.Properties.ContainsKey(StackKey))
                {
                    stack = context.ActionDescriptor.Properties[StackKey] as Stack<IDisposable>;
                }
                else
                {
                    stack = new Stack<IDisposable>();
                    context.ActionDescriptor.Properties[StackKey] = (object)stack;
                }
                MiniProfiler current = MiniProfiler.Current;
                if (current != null)
                {
                    string controllerName = context.Controller.GetType().Name;
                    string actionName = context.ActionDescriptor.DisplayName;
                    stack.Push(MiniProfiler.Current.Step("Controller: " + controllerName + actionName));
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ActionDescriptor.Properties.ContainsKey(StackKey))
            {
                var stack = context.ActionDescriptor.Properties[StackKey] as Stack<IDisposable>;
                if (stack != null && stack.Count > 0)
                {
                    stack.Pop().Dispose();
                }
            }

            MiniProfiler.Current.Stop();
        }
    }
}
