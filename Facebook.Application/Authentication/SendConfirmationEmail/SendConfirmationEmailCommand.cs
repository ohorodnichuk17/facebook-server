using ErrorOr;
using MediatR;

namespace Facebook.Application.Authentication.SendConfirmationEmail;

public record SendConfirmationEmailCommand(
    string Email, string BaseUrl) : IRequest<ErrorOr<Success>>;
