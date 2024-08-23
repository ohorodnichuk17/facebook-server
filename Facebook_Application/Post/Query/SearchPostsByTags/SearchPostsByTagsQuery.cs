using ErrorOr;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Query.SearchPostsByTags;

public record SearchPostsByTagsQuery(string Tag) : IRequest<ErrorOr<List<PostEntity>>>;