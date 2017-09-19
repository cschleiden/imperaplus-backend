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
            RecurringJob.AddOrUpdate<TimeoutJob>("Timeouts", x => x.Handle(), Cron.MinuteInterval(2));

            // Ladders
            RecurringJob.AddOrUpdate<LadderJob>("Ladders", x => x.Handle(), Cron.MinuteInterval(2));

            // Tournaments
            RecurringJob.AddOrUpdate<TournamentStartJob>("TournamentsOpen", x => x.Handle(), Cron.Hourly);
            RecurringJob.AddOrUpdate<TournamentJob>("Tournaments", x => x.Handle(), Cron.MinuteInterval(5));

            // Cleanups
            RecurringJob.AddOrUpdate<UserCleanupJob>(UserCleanupJob.JobId, x => x.Handle(), Cron.Daily);
        }
    }
}
