using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.User;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Domain.Post;
using MediatR;

namespace Facebook.Application.Post.Command.Create;

public class CreatePostCommandHandler(
        IPostRepository postRepository,
        IImageStorageService imageStorageService,
        IUserRepository userRepository) : IRequestHandler<CreatePostCommand, ErrorOr<Unit>>
{
    public async Task<ErrorOr<Unit>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId.ToString());

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
        };

        var postResult = await postRepository.CreatePostAsync(post);

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

        var result = await postRepository.UpdatePostAsync(post);

        if (result.IsError)
        {
            return result.Errors;
        }

        return Unit.Value;
    }
}