using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Status.Block;

public class BlockUnblockUserCommandValidator : AbstractValidator<BlockUnblockUserCommand>
{
    public BlockUnblockUserCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId must not be empty");
    }
}
