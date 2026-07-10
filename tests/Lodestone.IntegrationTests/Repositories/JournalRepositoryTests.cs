using FluentAssertions;
using Lodestone.Domain.Entities;
using Lodestone.Infrastructure.Data;
using Lodestone.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Lodestone.IntegrationTests.Repositories;

public class JournalRepositoryTests
{
    [Fact]
    public async Task GetByStudentIdAsync_ReturnsOnlyActiveOwnedEntriesNewestFirst()
    {
        await using var context = CreateContext();
        context.MoodJournalEntries.AddRange(
            Entry(1, 7, new DateTime(2026, 7, 10, 8, 0, 0, DateTimeKind.Utc)),
            Entry(2, 7, new DateTime(2026, 7, 11, 8, 0, 0, DateTimeKind.Utc)),
            Entry(3, 8, new DateTime(2026, 7, 12, 8, 0, 0, DateTimeKind.Utc)),
            Entry(4, 7, new DateTime(2026, 7, 13, 8, 0, 0, DateTimeKind.Utc), isDeleted: true));
        await context.SaveChangesAsync();
        var repository = new JournalRepository(context);

        var result = await repository.GetByStudentIdAsync(7);

        result.Select(entry => entry.Id).Should().ContainInOrder(2, 1);
        result.Should().OnlyContain(entry => entry.StudentProfileId == 7 && !entry.IsDeleted);
    }

    [Fact]
    public async Task AddAsync_AddsEntryToContext()
    {
        await using var context = CreateContext();
        var repository = new JournalRepository(context);
        var entry = Entry(0, 7, DateTime.UtcNow);

        await repository.AddAsync(entry);
        await context.SaveChangesAsync();

        context.MoodJournalEntries.Should().ContainSingle(value => value.StudentProfileId == 7);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"journal-tests-{Guid.NewGuid()}")
            .Options;
        return new ApplicationDbContext(options);
    }

    private static MoodJournalEntry Entry(int id, int profileId, DateTime date, bool isDeleted = false)
        => new()
        {
            Id = id,
            StudentProfileId = profileId,
            MoodRating = 3,
            EntryDateUtc = date,
            CreatedAtUtc = date,
            IsDeleted = isDeleted
        };
}
