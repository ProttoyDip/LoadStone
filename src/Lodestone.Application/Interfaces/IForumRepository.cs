using Lodestone.Domain.Entities;

namespace Lodestone.Application.Interfaces;

/// <summary>Forum-specific queries. Implemented in Infrastructure.</summary>
public interface IForumRepository
{
    Task<IReadOnlyList<ForumCategory>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ForumPost>> GetPostsByCategoryAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<ForumPost?> GetPostWithCommentsAsync(int postId, CancellationToken cancellationToken = default);
    Task AddPostAsync(ForumPost post, CancellationToken cancellationToken = default);
    Task AddCommentAsync(ForumComment comment, CancellationToken cancellationToken = default);
    Task AddFlagAsync(ForumFlag flag, CancellationToken cancellationToken = default);
    Task<ForumPost?> GetPostByIdAsync(int postId, CancellationToken cancellationToken = default);
}
