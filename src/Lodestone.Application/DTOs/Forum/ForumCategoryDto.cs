namespace Lodestone.Application.DTOs.Forum;

/// <summary>Summary of a forum category with post count.</summary>
public record ForumCategoryDto(int Id, string Name, string? Description, int PostCount);

/// <summary>A forum post with its comments fully populated.</summary>
public record ForumPostDetailDto(
    int Id,
    int CategoryId,
    string CategoryName,
    string AuthorUserId,
    string Title,
    string Body,
    DateTime CreatedAtUtc,
    IReadOnlyList<ForumCommentDto> Comments);
