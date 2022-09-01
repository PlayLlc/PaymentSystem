using AutoMapper;
using MerchantPortal.Application.DTO;
using MerchantPortal.Application.DTO.PointOfSale;
using MerchantPortal.Core.Entities;
using MerchantPortal.Core.Entities.PointOfSale;

namespace MerchantPortal.Infrastructure.Persistence;

public class PersistenceMapperProfile : Profile
{
    public PersistenceMapperProfile()
    {
        CreateMap<CompanyEntity, CompanyDto>();
        CreateMap<MerchantEntity, MerchantDto>();
        CreateMap<StoreEntity, StoreDto>();
        CreateMap<TerminalEntity, TerminalDto>();

        CreateMap<PoSConfiguration, PoSConfigurationDto>();
        CreateMap<CertificateAuthorityConfiguration, CertificateAuthorityConfigurationDto>();
        CreateMap<CertificateConfiguration, CertificateConfigurationDto>();
        CreateMap<Combination, CombinationDto>();
        CreateMap<DisplayConfiguration, DisplayConfigurationDto>();
        CreateMap<DisplayMessageSet, DisplayMessageSetDto>();
        CreateMap<DisplayMessage, DisplayMessageDto>();
        CreateMap<KernelConfiguration, KernelConfigurationDto>();
        CreateMap<PosConfigurationHeader, PosConfigurationHeaderDto>();
        CreateMap<ProximityCouplingDeviceConfiguration, ProximityCouplingDeviceConfigurationDto>();
        CreateMap<SequenceConfiguration, SequenceConfigurationDto>();
        CreateMap<TagLengthValue, TagLengthValueDto>();
        CreateMap<TerminalConfiguration, TerminalConfigurationDto>();

        CreateMap<PoSConfigurationDto, PoSConfiguration>();
        CreateMap<CertificateAuthorityConfigurationDto, CertificateAuthorityConfiguration>();
        CreateMap<CertificateConfigurationDto, CertificateConfiguration>();
        CreateMap<CombinationDto, Combination>();
        CreateMap<DisplayConfigurationDto, DisplayConfiguration>();
        CreateMap<DisplayMessageSetDto, DisplayMessageSet>();
        CreateMap<DisplayMessageDto, DisplayMessage>();
        CreateMap<KernelConfigurationDto, KernelConfiguration>();
        CreateMap<PosConfigurationHeaderDto, PosConfigurationHeader>();
        CreateMap<ProximityCouplingDeviceConfigurationDto, ProximityCouplingDeviceConfiguration>();
        CreateMap<SequenceConfigurationDto, SequenceConfiguration>();
        CreateMap<TagLengthValueDto, TagLengthValue>();
        CreateMap<TerminalConfigurationDto, TerminalConfiguration>();
    }
}
