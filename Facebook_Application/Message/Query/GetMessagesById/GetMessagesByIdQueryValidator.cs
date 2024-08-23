using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Query.GetMessagesById;

public class GetMessagesByIdQueryValidator : AbstractValidator<GetMessagesByIdQuery>
{
    public GetMessagesByIdQueryValidator()
    {
        RuleFor(r => r.ChatId)
            .NotEmpty().WithMessage("Chat id must not be empty");
    }
}
