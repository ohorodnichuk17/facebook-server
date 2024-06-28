﻿using Facebook.Application.UserProfile.Command.Delete;
using Facebook.Application.UserProfile.Command.Edit;
using Facebook.Application.UserProfile.Query.GetById;
using Facebook.Contracts.UserProfile.DeleteUser;
using Facebook.Contracts.UserProfile.EditUserProfile;
using Facebook.Contracts.UserProfile.GetUserProfileById;
using Mapster;

namespace Facebook.Server.Common.Mapping;

public class UserProfileMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(UserEditProfileRequest request, byte[] CoverPhoto, byte[] Avatar), UserEditProfileCommand>()
            .Map(dest => dest.CoverPhoto, src => src.CoverPhoto)
            .Map(dest => dest.Avatar, src => src.Avatar)
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<DeleteUserRequest, DeleteUserCommand>()
            .Map(dest => dest.UserId, src => src.UserId);

        config.NewConfig<GetUserProfileByIdRequest, GetUserProfileByIdQuery>();
    }
}
