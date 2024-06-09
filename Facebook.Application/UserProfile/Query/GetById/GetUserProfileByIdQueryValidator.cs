using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Query.GetById;

public class GetUserProfileByIdQueryValidator : AbstractValidator<GetUserProfileByIdQuery>
{
    public GetUserProfileByIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required.");
    }

}
