using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.Commands.Register;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Queries;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Mapster;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Facebook.Contracts.Authentication.Login.LoginRequest;
using RegisterRequest = Facebook.Contracts.Authentication.Register.RegisterRequest;

namespace Facebook.Server.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(RegisterRequest registerRequest, string BaseUrl), RegisterCommand>()
            .Map(dest => dest.BaseUrl, src => src.BaseUrl)
            .Map(dest => dest, src => src.registerRequest);
        
		
        config.NewConfig<ConfirmEmailRequest, ConfirmEmailCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();
        
        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Token, src => src.Token)
            .Map(dest => dest.Birthday, src => src.User.Birthday.ToString("yyyy-MM-dd")) 
            .Map(dest => dest, src => src.User);
        

        config.NewConfig<LoginRequest, LoginQuery>();
        
        config.NewConfig<(ForgotPasswordRequest registerRequest, string BaseUrl), ForgotPasswordQuery>()
            .Map(dest => dest.BaseUrl, src => src.BaseUrl)
            .Map(dest => dest, src => src.registerRequest);

        config.NewConfig<ResetPasswordRequest, ResetPasswordCommand>();

        config.NewConfig<ChangeEmailRequest, ChangeEmailCommand>();

    }
}