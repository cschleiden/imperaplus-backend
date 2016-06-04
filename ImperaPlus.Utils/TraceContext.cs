using System;
using NLog.Fluent;
using StackExchange.Profiling;

namespace ImperaPlus.Utils
{
    public static class TraceContext
    {
        public static void Trace(string name, Action action)
        {
            Log.Info().Message("Entering {0}", name).Write();

            using (MiniProfiler.Current.Step(name))
            {
                action();
            }

            Log.Info().Message("Leaving {0}", name).Write();
        }

        public static IDisposable Trace(string name)
        {
            return MiniProfiler.Current.Step(name);
        }
    }
}
