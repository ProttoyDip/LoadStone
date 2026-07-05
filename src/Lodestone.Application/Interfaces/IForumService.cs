using Lodestone.Application.DTOs.Forum;

namespace Lodestone.Application.Interfaces;

public interface IForumService
{
    Task<IReadOnlyList<ForumPostDto>> GetPostsAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<ForumPostDto> CreatePostAsync(CreateForumPostDto dto, CancellationToken cancellationToken = default);
    Task<ForumCommentDto> AddCommentAsync(CreateForumCommentDto dto, CancellationToken cancellationToken = default);
    Task FlagPostAsync(int postId, string reason, CancellationToken cancellationToken = default);
}
