using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Chat.Query.GetChatsByUserId;

public class GetChatsByUserIdQueryValidator : AbstractValidator<GetChatsByUserIdQuery>
{
    public GetChatsByUserIdQueryValidator()
    {
        RuleFor(r => r.UserId)
            .NotEmpty().WithMessage("User id must not be empty");
    }
}
