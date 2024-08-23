using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MapsterMapper;
using MediatR;

namespace Facebook.Application.Post.Command.Create;

public class CreatePostCommandHandler(
        IUnitOfWork unitOfWork,
        IImageStorageService imageStorageService,
        ICurrentUserService currentUserService,
        IMapper mapper) : IRequestHandler<CreatePostCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.GetCurrentUserId();
        var user = await unitOfWork.User.GetUserByIdAsync(currentUserId);

        if (user.IsError)
        {
            return user.Errors;
        }

        var userResult = user.Value;

        var post = mapper.Map<PostEntity>(request);
        post.UserId = new Guid(currentUserId);

        var postResult = await unitOfWork.Post.CreateAsync(post);

        if (postResult.IsError)
        {
            return postResult.Errors;
        }

        if (request.Images != null && request.Images.Any())
        {
            var imageNames = await imageStorageService.AddPostImagesAsync(request.Images.Select(i => i.Image).ToList());

            var imagesEntities = request.Images.Select((image, index) => new ImagesEntity
            {
                PostId = post.Id,
                PriorityImage = image.Priority,
                ImagePath = imageNames[index]
            }).ToList();

            post.Images = imagesEntities;
        }

        var result = await unitOfWork.Post.UpdatePostAsync(post);

        if (result.IsError)
        {
            return result.Errors;
        }

        return Unit.Value;
    }
}