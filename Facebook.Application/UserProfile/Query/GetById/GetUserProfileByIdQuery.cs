using ErrorOr;
using Facebook.Domain.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Query.GetById;

public record GetUserProfileByIdQuery(Guid UserId) : IRequest<ErrorOr<UserProfileEntity>>;