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
            RecurringJob.AddOrUpdate<TimeoutJob>("RunTimeouts", x => x.Handle(null), "*/2 * * * *");

            // Ladders
            RecurringJob.AddOrUpdate<LadderJob>("SyncLadders", x => x.Handle(null), "*/4 * * * *");

            // Tournaments
            RecurringJob.AddOrUpdate<TournamentStartJob>("StartTournaments", x => x.Handle(null), Cron.Hourly);
            RecurringJob.AddOrUpdate<TournamentJob>("SyncTournaments", x => x.Handle(null), "*/5 * * * *");

            // Cleanups
            RecurringJob.AddOrUpdate<UserCleanupJob>("Cleanup users", x => x.Handle(null), Cron.Daily);
            RecurringJob.AddOrUpdate<GameCleanupJob>("Cleanup games", x => x.Handle(null), Cron.Hourly);
            RecurringJob.AddOrUpdate<TokenCleanupJob>("Cleanup tokens", x => x.Handle(null), Cron.Daily);

            // Manual
            RecurringJob.AddOrUpdate<LadderScorejob>("Score ladders", x => x.Handle(null), "0 0 31 2 0");
        }
    }
}
