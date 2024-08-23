using ErrorOr;
using MediatR;

namespace Facebook.Application.Authentication.ConfirmEmail;

public record ConfirmEmailCommand(
	Guid UserId,
	string ValidEmailToken) : IRequest<ErrorOr<string>>;
