using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Chat.Command.Delete;

public class DeleteChatByIdCommandValidator : AbstractValidator<DeleteChatByIdCommand>
{
    public DeleteChatByIdCommandValidator()
    {
        RuleFor(r => r.Id)
            .NotEmpty().WithMessage("Id must not be empty");
    }
}
