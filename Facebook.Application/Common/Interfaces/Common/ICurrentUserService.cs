namespace Facebook.Application.Common.Interfaces.Common;

public interface ICurrentUserService
{
    string GetCurrentUserRole();
    string GetCurrentUserId();
}