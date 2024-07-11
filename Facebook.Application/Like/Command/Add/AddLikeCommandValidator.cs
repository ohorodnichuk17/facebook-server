using Facebook.Application.Story.Command.Delete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Like.Command.Add;

public class AddLikeCommandValidator : AbstractValidator<AddLikeCommand>
{
    public AddLikeCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");
        RuleFor(x => x.PostId)
                .NotEmpty()
                .WithMessage("Post id is required.");
    }
}
