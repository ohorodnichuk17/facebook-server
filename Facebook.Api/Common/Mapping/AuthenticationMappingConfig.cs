using Facebook.Application.Authentication.Commands.Register;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.Queries;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Facebook.Contracts.Authentication.Login;
using Facebook.Contracts.Authentication.Register;
using Mapster;

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

        // config.NewConfig<(ForgotPasswordRequest registerRequest, string BaseUrl), ForgotPasswordQuery>()
        //     .Map(dest => dest.BaseUrl, src => src.BaseUrl)
        //     .Map(dest => dest, src => src.registerRequest);

        config.NewConfig<LoginRequest, LoginQuery>();

    }
}