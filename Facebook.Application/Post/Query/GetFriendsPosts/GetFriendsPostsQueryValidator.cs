using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Post.Query.GetFriendsPosts;

public class GetFriendsPostsQueryValidator : AbstractValidator<GetFriendsPostsQuery>
{
    public GetFriendsPostsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User id is required.");
    }
}
