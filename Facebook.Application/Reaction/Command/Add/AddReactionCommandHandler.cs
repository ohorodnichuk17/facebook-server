﻿using ErrorOr;
using Facebook.Application.Common.Interfaces.Common;
using Facebook.Application.Common.Interfaces.Post.IRepository;
using Facebook.Application.Common.Interfaces.Reaction.IRepository;
using Facebook.Application.Common.Interfaces.User.IRepository;
using Facebook.Application.Story.Command.Create;
using Facebook.Domain.Post;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Reaction.Command.Add;

public class AddReactionCommandHandler(
    IReactionRepository reactionRepository,
    IUserRepository userRepository,
    IPostRepository postRepository) : IRequestHandler<AddReactionCommand, ErrorOr<ReactionEntity>>
{
    public async Task<ErrorOr<ReactionEntity>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId.ToString());
        var post = await postRepository.GetPostByIdAsync(request.PostId);

        if (user.IsError)
        {
            return user.Errors;
        }
        if (post.IsError)
        {
            return post.Errors;
        }

        var reaction = new ReactionEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Value.Id,
            PostId = post.Value.Id,
            CreatedAt = DateTime.Now,
            TypeCode = request.TypeCode
        };

        var reactionResult = await reactionRepository.AddReactionAsync(reaction);

        if (reactionResult.IsError)
        {
            return reactionResult.Errors;
        }

        return reactionResult;
    }
}
