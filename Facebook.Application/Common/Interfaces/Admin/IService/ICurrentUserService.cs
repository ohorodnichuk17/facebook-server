namespace Facebook.Application.Common.Interfaces.Admin.IService;

public interface ICurrentUserService
{
    string GetCurrentUserRole();
    string GetCurrentUserId();
}