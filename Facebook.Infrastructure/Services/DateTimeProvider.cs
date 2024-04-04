using Facebook.Application.Common.Interfaces.Services;

namespace Facebook.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}