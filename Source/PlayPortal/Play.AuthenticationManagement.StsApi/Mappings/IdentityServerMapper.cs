using AutoMapper;
using IdentityServer4;
using IdentityServer4.Test;
using Play.AuthenticationManagement.Identity.Services;
using Play.AuthenticationManagement.IdentityServer.Models.Account;

namespace Play.AuthenticationManagement.IdentityServer.Mappings;

public class IdentityServerMapper : Profile
{
    public IdentityServerMapper()
    {
        CreateMap<TestUser, IdentityServerUser>()
            .ForMember(dest => dest.AdditionalClaims, opt => opt.MapFrom(src => src.Claims));

        CreateMap<RegisterViewModel, CreateUserInput>();
    }
}
