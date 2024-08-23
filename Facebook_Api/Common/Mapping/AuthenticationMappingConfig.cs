using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Login;
using Facebook.Application.Authentication.Register;
using Facebook.Application.Authentication.ResendConfirmEmail;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Application.DTO;
using Facebook.Application.DTO_s;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common.Response;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Facebook.Contracts.Authentication.ResendConfirmEmail;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Facebook.Contracts.Authentication.Login.LoginRequest;
using RegisterRequest = Facebook.Contracts.Authentication.Register.RegisterRequest;

namespace Facebook.Server.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(RegisterRequest registerRequest, string BaseUrl, byte[] Image), RegisterCommand>()
            .Map(dest => dest.BaseUrl, src => src.BaseUrl)
            .Map(dest => dest.Avatar, src => src.Image)
            .Map(dest => dest, src => src.registerRequest);

        config.NewConfig<ConfirmEmailRequest, ConfirmEmailCommand>();

        config.NewConfig<(ResendConfirmEmailRequest request, string BaseUrl), ResendConfirmEmailCommand>()
            .Map(dest => dest.BaseUrl, src => src.BaseUrl)
            .Map(dest => dest, src => src.request);

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User)
            .Map(dest => dest.Birthday, src => src.User.Birthday.ToString("yyyy-MM-dd"));

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<(ForgotPasswordRequest registerRequest, string BaseUrl), ForgotPasswordQuery>()
            // .Map(dest => dest.Email, src => src.registerRequest.Email)
            .Map(dest => dest.BaseUrl, src => src.BaseUrl)
            .Map(dest => dest, src => src.registerRequest);

        config.NewConfig<ResetPasswordRequest, ResetPasswordCommand>();

        config.NewConfig<ChangeEmailRequest, ChangeEmailCommand>();

        config.NewConfig<(ChangeEmailRequest changeEmailRequest, string BaseUrl), ChangeEmailCommand>()
            .Map(dest => dest.Email, src => src.changeEmailRequest.Email)
            .Map(dest => dest.BaseUrl, src => src.BaseUrl);

        config.NewConfig<UserDto, UserDto>()
            .Map(dest => dest.Posts, src => src.Posts.Adapt<List<PostDto>>())
            .Map(dest => dest.Stories, src => src.Stories.Adapt<List<StoryDto>>());
    }
}