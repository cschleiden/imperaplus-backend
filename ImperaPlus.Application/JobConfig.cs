using Hangfire;
using ImperaPlus.Application.Jobs;

namespace ImperaPlus.Application
{
    public class JobConfig
    {
        /// <summary>
        /// Configure recurring jobs
        /// </summary>
        public static void Configure()
        {
            // Game timeouts
            RecurringJob.AddOrUpdate<TimeoutJob>("Timeouts", x => x.Handle(null), "*/2 * * * *");

            // Ladders
            RecurringJob.AddOrUpdate<LadderJob>("Ladders", x => x.Handle(null), "*/2 * * * *");

            // Tournaments
            // RecurringJob.AddOrUpdate<TournamentStartJob>("TournamentsOpen", x => x.Handle(null), Cron.Hourly);
            // RecurringJob.AddOrUpdate<TournamentJob>("Tournaments", x => x.Handle(null), "*/5 * * * * *");

            // Cleanups
            RecurringJob.AddOrUpdate<UserCleanupJob>(UserCleanupJob.JobId, x => x.Handle(), Cron.Daily);
            RecurringJob.AddOrUpdate<GameCleanupJob>(GameCleanupJob.JobId, x => x.Handle(null), Cron.Hourly);

            // Manual
            RecurringJob.AddOrUpdate<LadderScorejob>(LadderScorejob.JobId, x => x.Handle(null), "0 0 31 2 0");
        }
    }
}
