using Autofac;
using System.Threading.Tasks;

namespace ImperaPlus.Application.Jobs
{
    public abstract class Job
    {
        protected ILifetimeScope LifetimeScope { get; private set; }

        public Job(ILifetimeScope scope)
        {
            this.LifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");
        }

        public abstract void Handle();
    }

    public abstract class BackgroundJob
    {
        protected ILifetimeScope LifetimeScope { get; private set; }

        public BackgroundJob(ILifetimeScope scope)
        {
            this.LifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");
        }
    }

    public abstract class AsyncJob
    {
        protected ILifetimeScope LifetimeScope { get; private set; }

        public AsyncJob(ILifetimeScope scope)
        {
            this.LifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");
        }

        public abstract Task Handle();
    }
}
