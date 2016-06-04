using Autofac;

namespace ImperaPlus.Application.Jobs
{
    public abstract class Job
    {
        protected ILifetimeScope LifetimeScope { get; private set; }

        public Job(ILifetimeScope scope)
        {
            this.LifetimeScope = scope.BeginLifetimeScope("AutofacWebRequest");
        }
    }
}
