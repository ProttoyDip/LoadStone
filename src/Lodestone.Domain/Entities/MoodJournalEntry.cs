using Lodestone.Domain.Common;

namespace Lodestone.Domain.Entities;

/// <summary>Optional private mood journal entry owned by a student.</summary>
public class MoodJournalEntry : SoftDeleteEntity
{
    public int StudentProfileId { get; set; }
    public StudentProfile? StudentProfile { get; set; }

    public int MoodRating { get; set; }        // e.g. 1..5
    public string? Note { get; set; }
    public DateTime EntryDateUtc { get; set; }
}
