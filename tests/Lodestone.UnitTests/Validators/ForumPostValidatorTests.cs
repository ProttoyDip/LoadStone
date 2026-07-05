using FluentAssertions;
using FluentValidation.TestHelper;
using Lodestone.Application.DTOs.Forum;
using Lodestone.Application.Validators;
using Xunit;

namespace Lodestone.UnitTests.Validators;

public class ForumPostValidatorTests
{
    private readonly ForumPostValidator _validator = new();

    [Fact]
    public void Empty_title_is_invalid()
    {
        var result = _validator.TestValidate(new CreateForumPostDto(1, string.Empty, "body"));
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }
}
