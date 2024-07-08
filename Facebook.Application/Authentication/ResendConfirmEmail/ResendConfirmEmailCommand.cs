using MediatR;
using ErrorOr;

namespace Facebook.Application.Authentication.ResendConfirmEmail;

public record ResendConfirmEmailCommand(
    string Email,
    string BaseUrl) : IRequest<ErrorOr<string>>;