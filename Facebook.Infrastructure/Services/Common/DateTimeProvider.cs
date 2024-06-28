using Facebook.Application.Common.Interfaces.Common;

namespace Facebook.Infrastructure.Services.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}