﻿using MerchantPortal.Application.DTO;
using MerchantPortal.Application.DTO.PointOfSale;

namespace MerchantPortal.Application.Contracts.Services;

public interface IPoSConfigurationService
{
    Task CreateNewPosConfiguration(CreatePosConfigurationDto initialConfiguration);

    Task<PoSConfigurationDto> GetTerminalPoSConfiguration(long terminalId);

    Task<PoSConfigurationDto> GetPoSConfiguration(long id);

    Task UpdatePosTerminalConfiguration(long id, TerminalConfigurationDto terminalConfiguration);

    Task AddPosCombinationConfiguration(long id, CombinationDto combination);

    Task UpdatePosKernelConfiguration(long id, KernelConfigurationDto kernelConfiguration);

    Task UpdatePosDisplayConfiguration(long id, DisplayConfigurationDto displayConfiguration);

    Task UpdateProximityCouplingDeviceConfiguration(long id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration);

    Task UpdateCertificateConfiguration(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration);

    Task AddCertificateConfiguration(long id, CertificateConfigurationDto certificateConfiguration);
}
