namespace Lodestone.Application.DTOs.Journal;

public record JournalEntryDto(int Id, int StudentProfileId, int MoodRating, string? Note, DateTime EntryDateUtc);

public record CreateJournalEntryDto(int MoodRating, string? Note);
