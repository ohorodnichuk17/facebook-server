using Facebook.Application.UserProfile.Command.DeleteUser;
using Facebook.Application.UserProfile.Command.Edit;
using Facebook.Application.UserProfile.Common;
using Facebook.Application.UserProfile.Query.GetById;
using Facebook.Contracts.DeleteRequest;
using Facebook.Contracts.UserProfile.Edit;
using Facebook.Contracts.UserProfile.GetById;
using Facebook.Domain.User;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class UserProfileMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(UserEditProfileRequest request, byte[] CoverPhoto, byte[] Avatar), UserEditProfileCommand>()
            .Map(dest => dest.CoverPhoto, src => src.CoverPhoto)
            .Map(dest => dest.Avatar, src => src.Avatar)
            .Map(dest => dest.isOnline, src => src.request.isOnline)
            .Map(dest => dest, src => src.request);

        config.NewConfig<DeleteRequest, DeleteUserCommand>()
            .Map(dest => dest.UserId, src => src.Id);

        config.NewConfig<GetUserProfileByIdRequest, GetUserProfileByIdQuery>();
        // config.NewConfig<GetUserProfileByIdRequest, BlockUnblockUserCommand>()
        //     .Map(dest => dest.UserId, src => src.UserId);

        config.NewConfig<(UserProfileEntity profile, string token), EditProfileResult>()
            .Map(dest => dest.Token, src => src.token)
            .Map(dest => dest.UserProfile, src => src.profile);
    }
}
