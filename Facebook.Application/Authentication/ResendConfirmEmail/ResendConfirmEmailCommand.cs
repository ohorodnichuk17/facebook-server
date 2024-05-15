using MediatR;
using ErrorOr;

namespace Facebook.Application.Authentication.ResendConfirmEmail;

public record ResendConfirmEmailCommand(
    Guid UserId,
    string ValidEmailToken,
    string BaseUrl) : IRequest<ErrorOr<string>>;