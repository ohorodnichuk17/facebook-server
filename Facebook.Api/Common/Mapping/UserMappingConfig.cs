using Facebook.Application.Users.Common.ChangePassword;
using Facebook.Contracts.User.Common.ChangePassword;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(ChangePasswordRequest changePassword, string UserId),	ChangePasswordCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest, src => src.changePassword);
    }
}