using Facebook.Application.Comment.Command.Add;
using Facebook.Application.Comment.Command.AddReplyComment;
using Facebook.Application.Comment.Command.Delete;
using Facebook.Application.Comment.Command.Edit;
using Facebook.Contracts.Comment.AddReplyComment;
using Facebook.Contracts.Comment.Create;
using Facebook.Contracts.Comment.Edit;
using Facebook.Contracts.DeleteRequest;
using Facebook.Domain.Post;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class CommentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddCommentRequest, AddCommentCommand>();

        config.NewConfig<DeleteRequest, DeleteCommentCommand>()
            .Map(dest => dest.Id, src => src.Id);

        config.NewConfig<AddCommentCommand, CommentEntity>()
            .Map(dest => dest.CreatedAt, src => DateTime.Now);

        config.NewConfig<EditCommentRequest, EditCommentCommand>();

        config.NewConfig<AddReplyCommentRequest, AddReplyCommentCommand>();
    }
}