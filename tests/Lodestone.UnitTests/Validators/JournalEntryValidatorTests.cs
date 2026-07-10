using FluentAssertions;
using Lodestone.Application.DTOs.Journal;
using Lodestone.Application.Validators;
using Xunit;

namespace Lodestone.UnitTests.Validators;

public class JournalEntryValidatorTests
{
    private readonly JournalEntryValidator _validator = new();

    [Fact]
    public void Validate_AcceptsRatingAndOptionalNote()
    {
        var result = _validator.Validate(new CreateJournalEntryDto(4, "A manageable day."));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Validate_RejectsRatingOutsideOneToFive(int rating)
    {
        var result = _validator.Validate(new CreateJournalEntryDto(rating, null));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == nameof(CreateJournalEntryDto.MoodRating));
    }

    [Fact]
    public void Validate_RejectsNoteLongerThanTwoThousandCharacters()
    {
        var result = _validator.Validate(new CreateJournalEntryDto(3, new string('a', 2001)));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(error => error.PropertyName == nameof(CreateJournalEntryDto.Note));
    }
}
