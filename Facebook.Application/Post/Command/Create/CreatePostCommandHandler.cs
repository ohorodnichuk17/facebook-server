using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.IUnitOfWork;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Command.Create;

public class CreatePostCommandHandler(
        IUnitOfWork unitOfWork,
        IImageStorageService imageStorageService) : IRequestHandler<CreatePostCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.User.GetUserByIdAsync(request.UserId.ToString());

        if (user.IsError)
        {
            return user.Errors;
        }

        var userResult = user.Value;

        var post = new PostEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Content = request.Content,
            Tags = request.Tags,
            Location = request.Location,
            IsArchive = request.IsArchive,
            UserId = request.UserId,
            CreatedAt = DateTime.Now,
            FeelingId = request.FeelingId,
        };

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