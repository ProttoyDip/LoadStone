using Lodestone.Application.DTOs.Journal;

namespace Lodestone.Web.ViewModels.Journal;

public class JournalViewModel
{
    public IReadOnlyList<JournalEntryDto> Entries { get; set; } = new List<JournalEntryDto>();
    public CreateJournalEntryDto NewEntry { get; set; } = new(3, null);
    public bool HasEntryToday { get; set; }
}
