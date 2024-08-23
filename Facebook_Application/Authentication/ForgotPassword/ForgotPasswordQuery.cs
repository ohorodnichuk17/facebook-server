using ErrorOr;
using MediatR;

namespace Facebook.Application.Authentication.ForgotPassword;

public record ForgotPasswordQuery(
    string Email,
    string BaseUrl) : IRequest<ErrorOr<Success>>;
