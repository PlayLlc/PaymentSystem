using AutoMapper;
using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Domain.Entities.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale;

internal class PoSConfigurationService : IPoSConfigurationService
{
    private readonly IPoSRepository _posRepository;
    private readonly IMapper _mapper;

    public PoSConfigurationService(IPoSRepository posRepository, IMapper mapper)
    {
        _posRepository = posRepository;
        _mapper = mapper;
    }

    public async Task CreateNewPosConfiguratioAsync(CreatePosConfigurationDto initialConfiguration)
    {
        var entity = new PoSConfiguration()
        {
            CompanyId = initialConfiguration.CompanyId,
            MerchantId = initialConfiguration.MerchantId,
            StoreId = initialConfiguration.StoreId,
            TerminalId = initialConfiguration.TerminalId
        };

        await _posRepository.InsertNewPosConfigurationAsync(entity);
    }

    public async Task UpdateCertificateConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);
        
        var entity = _mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.CertificateAuthorityConfiguration, entity)
            });
    }

    public async Task AddPosCombinationConfigurationAsync(long id, CombinationDto combination)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<Combination>(combination);

        await _posRepository.AddCombinationConfigurationAsync(id, entity);
    }

    public async Task UpdatePosCombinationsConfigurationAsync(long id, IEnumerable<CombinationDto> combinations)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var collection = _mapper.Map<IEnumerable<Combination>>(combinations);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.Combinations, collection)
            });
    }

    public async Task UpdatePosDisplayConfigurationAsync(long id, DisplayConfigurationDto displayConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<DisplayConfiguration>(displayConfiguration);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.DisplayConfiguration, entity)
            });
    }

    public async Task UpdatePosKernelConfigurationAsync(long id, KernelConfigurationDto kernelConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<KernelConfiguration>(kernelConfiguration);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.KernelConfiguration, entity)
            });
    }

    public async Task UpdatePosTerminalConfigurationAsync(long id, TerminalConfigurationDto terminalConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<TerminalConfiguration>(terminalConfiguration);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.TerminalConfiguration, entity)
            });
    }

    public async Task UpdateProximityCouplingDeviceConfigurationAsync(long id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<ProximityCouplingDeviceConfiguration>(proximityCouplingDeviceConfiguration);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.ProximityCouplingDeviceConfiguration, entity)
            });
    }

    public async Task AddCertificateConfigurationAsync(long id, CertificateConfigurationDto certificateConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<CertificateConfiguration>(certificateConfiguration);

        await _posRepository.AddCertificateConfigurationAsync(id, entity);
    }

    public async Task UpdateCertificateAuthorityConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        bool exists = await _posRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var entity = _mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _posRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.CertificateAuthorityConfiguration, entity)
            });
    }

    public async Task<PoSConfigurationDto> GetTerminalPoSConfigurationAsync(long terminalId)
    {
        PoSConfiguration? configuration = await _posRepository.FindByTerminalIdAsync(terminalId);

        return _mapper.Map<PoSConfigurationDto>(configuration);
    }

    public async Task<PoSConfigurationDto> GetPoSConfigurationAsync(long id)
    {
        PoSConfiguration? configuration = await _posRepository.FindByIdAsync(id);

        return _mapper.Map<PoSConfigurationDto>(configuration);
    }

    public async Task<IEnumerable<PoSConfigurationDto>> GetStorePoSConfigurationsAsync(long storeId)
    {
        IEnumerable<PoSConfiguration> result = await _posRepository.SelectPosConfigurationsByStoreIdAsync(storeId);

        return _mapper.Map<IEnumerable<PoSConfigurationDto>>(result);
    }

    public async Task<IEnumerable<PoSConfigurationDto>> GetMerchantPoSConfigurationsAsync(long merchantId)
    {
        IEnumerable<PoSConfiguration> result = await _posRepository.SelectPoSConfigurationsByMerchantIdAsync(merchantId);

        return _mapper.Map<IEnumerable<PoSConfigurationDto>>(result);
    }
}
