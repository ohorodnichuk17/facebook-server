using ErrorOr;
using Facebook.Domain.Chat;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook.Application.Message.Query.GetMessagesById;

public record GetMessagesByIdQuery(Guid ChatId) : IRequest<ErrorOr<IEnumerable<MessageEntity>>>;