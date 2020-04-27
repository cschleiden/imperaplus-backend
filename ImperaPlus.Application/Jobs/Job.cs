using Autofac;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using System;

namespace ImperaPlus.Application.Jobs
{
    public abstract class Job
    {
        protected ILifetimeScope LifetimeScope { get; private set; }
        protected JobLogger Log { get; private set; }

        public Job(ILifetimeScope scope)
        {
            this.LifetimeScope = scope;
        }

        public virtual void Handle(PerformContext performContext)
        {
            this.Log = new JobLogger(performContext);
        }
    }

    public abstract class BackgroundJob
    {
        protected ILifetimeScope LifetimeScope { get; private set; }

        public BackgroundJob(ILifetimeScope scope)
        {
            this.LifetimeScope = scope;
        }
    }

    public abstract class AsyncJob
    {
        protected ILifetimeScope LifetimeScope { get; private set; }
        protected JobLogger Log { get; private set; }

        public AsyncJob(ILifetimeScope scope)
        {
            this.LifetimeScope = scope;
        }

        public virtual Task Handle(PerformContext performContext)
        {
            this.Log = new JobLogger(performContext);

            return Task.CompletedTask;
        }
    }

    public class JobLogger : Domain.ILogger
    {
        private PerformContext performContext;

        public JobLogger(PerformContext performContext)
        {
            this.performContext = performContext;
        }

        public void Log(Domain.LogLevel level, string format, params object[] args)
        {
            // Send to hangfire
            var color = this.MapLevelToConsoleColor(level);
            this.performContext.WriteLine(color, format, args);

            // Persist
            NLog.Fluent.Log.Level(this.MapLevel(level)).Message(format, args).Write();
        }

        private ConsoleTextColor MapLevelToConsoleColor(Domain.LogLevel level)
        {
            if (level == Domain.LogLevel.Error)
            {
                return ConsoleTextColor.Red;
            }

            return ConsoleTextColor.White;
        }

        private NLog.LogLevel MapLevel(Domain.LogLevel level)
        {
            switch (level)
            {
                case Domain.LogLevel.Error:
                    return NLog.LogLevel.Error;
            }

            return NLog.LogLevel.Info;
        }
    }
}
