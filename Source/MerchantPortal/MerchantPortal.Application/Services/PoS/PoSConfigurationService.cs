﻿using AutoMapper;
using FluentValidation;
using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Application.DTO.PointOfSale;
using MerchantPortal.Core.Entities.PointOfSale;

namespace MerchantPortal.Application.Services.PoS;

internal class PoSConfigurationService : IPoSConfigurationService
{
    private readonly IPoSRepository _posRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<PoSConfigurationDto> _validator;

    public PoSConfigurationService(IPoSRepository posRepository, IMapper mapper, IValidator<PoSConfigurationDto> validator)
    {
        _posRepository = posRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task CreateNewPosConfiguration(CreatePosConfigurationDto initialConfiguration)
    {
        var entity = new PosConfigurationHeader()
        {
            CompanyId = initialConfiguration.CompanyId,
            MerchantId = initialConfiguration.MerchatId,
            StoreId = initialConfiguration.StoreId,
            TerminalId = initialConfiguration.TerminalId
        };

        await _posRepository.InsertPosConfigurationHeader(entity);
    }

    public async Task UpdateCertificateConfiguration(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        var entity = _mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.CertificateAuthorityConfiguration, entity)
            });
    }

    public async Task AddPosCombinationConfiguration(long id, CombinationDto combination)
    {
        var entity = _mapper.Map<Combination>(combination);

        await _posRepository.AddCombinationConfiguration(id, entity);
    }

    public async Task UpdatePosCombinationsConfiguration(long id, IEnumerable<CombinationDto> combinations)
    {
        var collection = _mapper.Map<IEnumerable<Combination>>(combinations);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.Combinations, collection)
            });
    }

    public async Task UpdatePosDisplayConfiguration(long id, DisplayConfigurationDto displayConfiguration)
    {
        var entity = _mapper.Map<DisplayConfiguration>(displayConfiguration);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.DisplayConfiguration, entity)
            });
    }

    public async Task UpdatePosKernelConfiguration(long id, KernelConfigurationDto kernelConfiguration)
    {
        var entity = _mapper.Map<KernelConfiguration>(kernelConfiguration);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.KernelConfiguration, entity)
            });
    }

    public async Task UpdatePosTerminalConfiguration(long id, TerminalConfigurationDto terminalConfiguration)
    {
        var entity = _mapper.Map<TerminalConfiguration>(terminalConfiguration);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.TerminalConfiguration, entity)
            });
    }

    public async Task UpdateProximityCouplingDeviceConfiguration(long id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration)
    {
        var entity = _mapper.Map<ProximityCouplingDeviceConfiguration>(proximityCouplingDeviceConfiguration);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.ProximityCouplingDeviceConfiguration, entity)
            });
    }

    public async Task AddCertificateConfiguration(long id, CertificateConfigurationDto certificateConfiguration)
    {
        var entity = _mapper.Map<CertificateConfiguration>(certificateConfiguration);

        await _posRepository.AddCertificateConfiguration(id, entity);
    }

    public async Task UpdateCertificateAuthorityConfiguration(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        var entity = _mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _posRepository.UpdateGivenFields(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.CertificateAuthorityConfiguration, entity)
            });
    }

    public async Task<PoSConfigurationDto> GetTerminalPoSConfiguration(long terminalId)
    {
        PoSConfiguration configuration = await _posRepository.FindByTerminalId(terminalId);

        return _mapper.Map<PoSConfigurationDto>(configuration);
    }

    public async Task<PoSConfigurationDto> GetPoSConfiguration(long id)
    {
        PoSConfiguration configuration = await _posRepository.FindById(id);

        return _mapper.Map<PoSConfigurationDto>(configuration);
    }

    public async Task<IEnumerable<PoSConfigurationDto>> GetStorePoSConfigurations(long storeId)
    {
        IEnumerable<PoSConfiguration> result = await _posRepository.SelectPosConfigurationsByStoreId(storeId);

        return _mapper.Map<IEnumerable<PoSConfigurationDto>>(result);
    }

    public async Task<IEnumerable<PoSConfigurationDto>> GetMerchantPoSConfigurations(long merchantId)
    {
        IEnumerable<PoSConfiguration> result = await _posRepository.SelectPoSConfigurationsByMerchantId(merchantId);

        return _mapper.Map<IEnumerable<PoSConfigurationDto>>(result);
    }
}
