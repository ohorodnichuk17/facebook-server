using Facebook.Application.Common.Interfaces.Services;

namespace Facebook.Infrastructure.Services.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}