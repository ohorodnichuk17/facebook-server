using ErrorOr;
using MediatR;

namespace Facebook.Application.User.Friends.Query.GetRelationshipsStatus;

public record GetRelationshipsStatusQuery(string FriendId) : IRequest<ErrorOr<RelationshipsStatus>>;
