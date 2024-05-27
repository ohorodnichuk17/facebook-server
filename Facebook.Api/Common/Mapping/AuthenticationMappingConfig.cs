using AutoMapper;
using Facebook.Application.Authentication.ChangeEmail;
using Facebook.Application.Authentication.Commands.Register;
using Facebook.Application.Authentication.Common;
using Facebook.Application.Authentication.ConfirmEmail;
using Facebook.Application.Authentication.ForgotPassword;
using Facebook.Application.Authentication.Queries;
using Facebook.Application.Authentication.ResendConfirmEmail;
using Facebook.Application.Authentication.ResetPassword;
using Facebook.Contracts.Authentication.ChangeEmail;
using Facebook.Contracts.Authentication.Common;
using Facebook.Contracts.Authentication.Common.Response;
using Facebook.Contracts.Authentication.ConfirmEmail;
using Microsoft.AspNetCore.Identity.Data;
using LoginRequest = Facebook.Contracts.Authentication.Login.LoginRequest;
using RegisterRequest = Facebook.Contracts.Authentication.Register.RegisterRequest;

namespace Facebook.Server.Common.Mapping;

public class AuthenticationMappingProfile : Profile
{
    public AuthenticationMappingProfile()
    {
        CreateMap<(RegisterRequest registerRequest, string BaseUrl, byte[] Image), RegisterCommand>()
            .ForMember(dest => dest.BaseUrl, opt => opt.MapFrom(src => src.BaseUrl))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.registerRequest));

        CreateMap<ConfirmEmailRequest, ConfirmEmailCommand>();

        CreateMap<(ConfirmEmailRequest request, string BaseUrl), ResendConfirmEmailCommand>()
            .ForMember(dest => dest.BaseUrl, opt => opt.MapFrom(src => src.BaseUrl))
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.request));

        CreateMap<LoginRequest, LoginQuery>();

        CreateMap<AuthenticationResult, AuthenticationResponse>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.User.Birthday.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.User));

        CreateMap<(ForgotPasswordRequest registerRequest, string BaseUrl), ForgotPasswordQuery>()
            .ForMember(dest => dest.BaseUrl, opt => opt.MapFrom(src => src.BaseUrl))
            .ForMember(dest => dest, opt => opt.MapFrom(src => src.registerRequest));

        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();

        CreateMap<ChangeEmailRequest, ChangeEmailCommand>();

        CreateMap<(ChangeEmailRequest changeEmailRequest, string BaseUrl), ChangeEmailCommand>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.changeEmailRequest.Email))
            .ForMember(dest => dest.BaseUrl, opt => opt.MapFrom(src => src.BaseUrl));
    }
}