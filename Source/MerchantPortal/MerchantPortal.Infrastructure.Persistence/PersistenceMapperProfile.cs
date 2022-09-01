using AutoMapper;
using MerchantPortal.Application.DTO;
using MerchantPortal.Core.Entities;

namespace MerchantPortal.Infrastructure.Persistence;

public class PersistenceMapperProfile : Profile
{
    public PersistenceMapperProfile()
    {
        CreateMap<CompanyEntity, CompanyDto>();
        CreateMap<MerchantEntity, MerchantDto>();
        CreateMap<StoreEntity, StoreDto>();
        CreateMap<TerminalEntity, TerminalDto>();

        CreateMap<CompanyDto, CompanyEntity>();
        CreateMap<MerchantDto, MerchantEntity>();
        CreateMap<StoreDto, StoreEntity>();
        CreateMap<TerminalDto, TerminalEntity>();
    }
}
