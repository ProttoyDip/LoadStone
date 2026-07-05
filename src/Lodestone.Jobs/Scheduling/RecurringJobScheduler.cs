using Hangfire;
using Lodestone.Jobs.BackgroundJobs;

namespace Lodestone.Jobs.Scheduling;

/// <summary>Registers the recurring Hangfire schedules. Called once at startup.</summary>
public static class RecurringJobScheduler
{
    public static void RegisterRecurringJobs(IRecurringJobManager recurringJobs)
    {
        recurringJobs.AddOrUpdate<WeeklyRiskScoringJob>(
            "weekly-risk-scoring", job => job.ExecuteAsync(CancellationToken.None), Cron.Weekly);

        recurringJobs.AddOrUpdate<NudgeNotificationJob>(
            "nudge-dispatch", job => job.ExecuteAsync(CancellationToken.None), Cron.Daily);

        recurringJobs.AddOrUpdate<BookingReminderJob>(
            "booking-reminders", job => job.ExecuteAsync(CancellationToken.None), Cron.Hourly);

        recurringJobs.AddOrUpdate<ForumModerationJob>(
            "forum-moderation", job => job.ExecuteAsync(CancellationToken.None), Cron.Daily);

        recurringJobs.AddOrUpdate<CrisisResourceEscalationJob>(
            "crisis-escalation", job => job.ExecuteAsync(CancellationToken.None), "*/15 * * * *");
    }
}
