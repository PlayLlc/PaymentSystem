using AutoMapper;
using IdentityServer4;
using IdentityServer4.Test;

namespace Play.AuthenticationManagement.IdentityServer.Mappings;

public class IdentityServerMapper : Profile
{
    public IdentityServerMapper()
    {
        CreateMap<TestUser, IdentityServerUser>()
            .ForMember(dest => dest.AdditionalClaims, opt => opt.MapFrom(src => src.Claims));
    }
}
