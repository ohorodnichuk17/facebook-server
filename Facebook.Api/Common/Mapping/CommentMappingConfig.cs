using Facebook.Application.Comment.Command.Add;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Contracts.Comment.Create;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Post;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class CommentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddCommentRequest, AddCommentCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.PostId, src => src.PostId);

        config.NewConfig<DeleteRequest, DeleteCommentCommand>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<AddCommentCommand, CommentEntity>()
            .Map(dest => dest.CreatedAt, src => DateTime.Now);
    }
}