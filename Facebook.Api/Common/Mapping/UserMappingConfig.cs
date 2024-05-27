using AutoMapper;
using Facebook.Application.Users.Common.ChangePassword;
using Facebook.Contracts.User.Common.ChangePassword;

namespace Facebook.Server.Common.Mapping;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<(ChangePasswordRequest changePassword, string UserId), ChangePasswordCommand>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.changePassword));
    }
}