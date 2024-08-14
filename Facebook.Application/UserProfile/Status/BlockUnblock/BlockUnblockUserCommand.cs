using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.UserProfile.Status.Block;

public record BlockUnblockUserCommand(Guid UserId) : IRequest<ErrorOr<bool>>;