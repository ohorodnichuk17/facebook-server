using ErrorOr;
using Facebook.Application.Common.Interfaces.IRepository;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Common.Interfaces.Like.IRepository;

public interface ILikeRepository : IRepository<LikeEntity>
{
    Task<ErrorOr<LikeEntity>> GetLikeByIdAsync(Guid id);
    Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikeByPostIdAsync(Guid postId);
    Task<ErrorOr<IEnumerable<LikeEntity>>> GetLikeByUserIdAsync(Guid userId);
    Task<ErrorOr<bool>> AddLikeAsync(LikeEntity like);
    Task<ErrorOr<bool>> DeleteLikeAsync(Guid id);
}
