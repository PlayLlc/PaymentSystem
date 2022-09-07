using AutoMapper;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;
using Play.MerchantPortal.Domain.Entities.PointOfSale;

namespace Play.MerchantPortal.Application.Mapping;

public class PoSConfigurationProfileMapper : Profile
{
    public PoSConfigurationProfileMapper()
    {
        CreateMap<PoSConfiguration, PoSConfigurationDto>();
        CreateMap<CertificateAuthorityConfiguration, CertificateAuthorityConfigurationDto>();
        CreateMap<CertificateConfiguration, CertificateConfigurationDto>();
        CreateMap<Combination, CombinationDto>();
        CreateMap<DisplayConfiguration, DisplayConfigurationDto>();
        CreateMap<DisplayMessageSet, DisplayMessageSetDto>();
        CreateMap<DisplayMessage, DisplayMessageDto>();
        CreateMap<KernelConfiguration, KernelConfigurationDto>();
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
        CreateMap<ProximityCouplingDeviceConfigurationDto, ProximityCouplingDeviceConfiguration>();
        CreateMap<SequenceConfigurationDto, SequenceConfiguration>();
        CreateMap<TagLengthValueDto, TagLengthValue>();
        CreateMap<TerminalConfigurationDto, TerminalConfiguration>();
    }
}
