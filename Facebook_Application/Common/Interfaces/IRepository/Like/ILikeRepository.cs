using ErrorOr;
using Facebook.Domain.Post;

namespace Facebook.Application.Common.Interfaces.IRepository.Like;

public interface ILikeRepository : IRepository<LikeEntity>
{
    Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikesByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikesByUserIdAsync(Guid userId);
    Task<ErrorOr<LikeEntity>> SaveIfNotExist(LikeEntity entity);
    Task<ErrorOr<bool>> DeleteByPostId(Guid userId, Guid postId);
}
