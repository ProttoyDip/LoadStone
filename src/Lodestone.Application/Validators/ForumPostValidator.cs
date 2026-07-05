using FluentValidation;
using Lodestone.Application.DTOs.Forum;

namespace Lodestone.Application.Validators;

public class ForumPostValidator : AbstractValidator<CreateForumPostDto>
{
    public ForumPostValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Body).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.CategoryId).GreaterThan(0);
    }
}
