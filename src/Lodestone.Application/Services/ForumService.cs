using Lodestone.Application.DTOs.Forum;
using Lodestone.Application.Interfaces;

namespace Lodestone.Application.Services;

public class ForumService : IForumService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUser;

    public ForumService(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public Task<IReadOnlyList<ForumPostDto>> GetPostsAsync(int categoryId, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<ForumPostDto> CreatePostAsync(CreateForumPostDto dto, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task<ForumCommentDto> AddCommentAsync(CreateForumCommentDto dto, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task FlagPostAsync(int postId, string reason, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
