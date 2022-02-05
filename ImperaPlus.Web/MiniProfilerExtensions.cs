using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace StackExchange.Profiling
{
    public static class MiniProfilerSpaExtensions
    {
        public static void MapMiniProfilerIncludes(this IEndpointRouteBuilder endpoints, RenderOptions? options = null, string url = "mini-profiler-includes")
        {
            endpoints.MapGet(url, async (context) =>
            {
                context.Response.ContentType = "text/html";
                var includes = MiniProfiler.Current.RenderIncludes(context, options);
                if (String.IsNullOrEmpty(includes.Value))
                    return;

                await context.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(includes.Value));
            });
        }
    }
}