using ErrorOr;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetFriendsPosts;

public record GetFriendsPostsQuery(Guid UserId) : IRequest<ErrorOr<IEnumerable<PostEntity>>>;