using Lodestone.Domain.Enums;

namespace Lodestone.Application.DTOs.Forum;

public record ForumPostDto(int Id, int CategoryId, string AuthorUserId, string Title, string Body, ForumPostStatus Status, DateTime CreatedAtUtc);

public record CreateForumPostDto(int CategoryId, string Title, string Body);
