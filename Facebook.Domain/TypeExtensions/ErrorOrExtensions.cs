using ErrorOr;

namespace Facebook.Domain.TypeExtensions;

public static class ErrorOrExtensions
{
	public static bool IsSuccess<T>(this ErrorOr<T> errorOr) => !errorOr.IsError;

}
