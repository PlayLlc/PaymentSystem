using AutoMapper;
using Play.MerchantPortal.Contracts.Messages.PointOfSale;
using Play.MerchantPortal.Domain.Entities.PointOfSale;

namespace Play.MerchantPortal.Application.Mapping;

public class PosConfigurationProfileMapper : Profile
{
    public PosConfigurationProfileMapper()
    {
        CreateMap<PointOfSaleConfiguration, PointOfSaleConfigurationDto>();
        CreateMap<CertificateAuthorityConfiguration, CertificateAuthorityConfigurationDto>();
        CreateMap<CertificateConfiguration, CertificateConfigurationDto>();
        CreateMap<CombinationConfiguration, CombinationConfigurationDto>();
        CreateMap<DisplayConfiguration, DisplayConfigurationDto>();
        CreateMap<DisplayMessageSet, DisplayMessageSetDto>();
        CreateMap<DisplayMessage, DisplayMessageDto>();
        CreateMap<KernelConfiguration, KernelConfigurationDto>();
        CreateMap<ProximityCouplingDeviceConfiguration, ProximityCouplingDeviceConfigurationDto>();
        CreateMap<SequenceConfiguration, SequenceConfigurationDto>();
        CreateMap<TagLengthValue, TagLengthValueDto>();
        CreateMap<TerminalConfiguration, TerminalConfigurationDto>();

        CreateMap<PointOfSaleConfigurationDto, PointOfSaleConfiguration>();
        CreateMap<CertificateAuthorityConfigurationDto, CertificateAuthorityConfiguration>();
        CreateMap<CertificateConfigurationDto, CertificateConfiguration>();
        CreateMap<CombinationConfigurationDto, CombinationConfiguration>();
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
