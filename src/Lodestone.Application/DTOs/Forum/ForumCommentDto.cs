namespace Lodestone.Application.DTOs.Forum;

public record ForumCommentDto(int Id, int PostId, string AuthorUserId, string Body, DateTime CreatedAtUtc);

public record CreateForumCommentDto(int PostId, string Body);
