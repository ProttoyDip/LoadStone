namespace Lodestone.Application.Exceptions;

/// <summary>Raised when a student already has an active journal entry for the current UTC day.</summary>
public class DailyJournalEntryLimitException : InvalidOperationException
{
    public DailyJournalEntryLimitException()
        : base("You have already completed today’s journal check-in.") { }
}
