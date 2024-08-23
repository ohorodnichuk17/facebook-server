using ErrorOr;
using Facebook.Domain.Chat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Chat.Query.GetChatsByUserId;

public record GetChatsByUserIdQuery(Guid UserId) : IRequest<ErrorOr<IEnumerable<ChatEntity>>>;