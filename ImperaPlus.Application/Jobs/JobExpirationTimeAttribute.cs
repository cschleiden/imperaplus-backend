using System;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace ImperaPlus.Application.Jobs
{
    public class JobExpirationTimeAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            context.JobExpirationTimeout = TimeSpan.FromHours(1);
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
        }
    }
}
