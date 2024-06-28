using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Command.Delete;

public record DeleteUserCommand(
    string UserId
) : IRequest<ErrorOr<bool>>;
