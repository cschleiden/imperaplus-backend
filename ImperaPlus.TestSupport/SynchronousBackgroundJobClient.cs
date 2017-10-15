using Hangfire;
using System;
using Hangfire.Common;
using Hangfire.States;
using Autofac;

namespace ImperaPlus.TestSupport
{
    public class SynchronousBackgroundJobClient : IBackgroundJobClient
    {
        private ILifetimeScope scope;

        public SynchronousBackgroundJobClient(ILifetimeScope scope)
        {
            this.scope = scope;
        }

        public bool ChangeState(string jobId, IState state, string fromState)
        {
            throw new NotImplementedException();
        }

        public string Create(Job job, IState state)
        {
            var activator = new AutofacJobActivator(scope);
            job.Perform(activator, new JobCancellationToken(false));

            return new Guid().ToString();
        }
    }
}
