using AutoMapper;
using FluentValidation;
using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Domain.Entities.PointOfSale;

namespace Play.MerchantPortal.Application.Services.PointsOfSale;

internal class PoSConfigurationService : IPoSConfigurationService
{
    private readonly IPoSRepository _PosRepository;
    private readonly IMapper _Mapper;

    private readonly IValidator<TerminalConfigurationDto> _TerminalConfigurationValidator;
    private readonly IValidator<ProximityCouplingDeviceConfigurationDto> _ProximityCouplingDeviceValidator;
    private readonly IValidator<CertificateAuthorityConfigurationDto> _CertificateAuthorityConfigurationValidator;
    private readonly IValidator<DisplayConfigurationDto> _DisplayConfigurationValidator;
    private readonly IValidator<KernelConfigurationDto> _KernelConfigurationValidator;

    public PoSConfigurationService(
        IPoSRepository posRepository,
        IMapper mapper,
        IValidator<TerminalConfigurationDto> terminalConfigurationValidator,
        IValidator<ProximityCouplingDeviceConfigurationDto> proximityCouplingDeviceValidator,
        IValidator<CertificateAuthorityConfigurationDto> certificateAuthorityConfigurationValidator,
        IValidator<DisplayConfigurationDto> displayConfigurationValidator,
        IValidator<KernelConfigurationDto> kernelConfigurationValidator)
    {
        _PosRepository = posRepository;
        _Mapper = mapper;
        _TerminalConfigurationValidator = terminalConfigurationValidator;
        _ProximityCouplingDeviceValidator = proximityCouplingDeviceValidator;
        _CertificateAuthorityConfigurationValidator = certificateAuthorityConfigurationValidator;
        _DisplayConfigurationValidator = displayConfigurationValidator;
        _KernelConfigurationValidator = kernelConfigurationValidator;
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

        await _PosRepository.InsertNewPosConfigurationAsync(entity);
    }

    public async Task UpdateCertificateConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);
        
        var entity = _Mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.CertificateAuthorityConfiguration, entity)
            });
    }

    public async Task UpdateCombinationsConfigurationAsync(long id, IEnumerable<CombinationDto> combinations)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var collection = _Mapper.Map<IEnumerable<Combination>>(combinations);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.Combinations, collection)
            });
    }

    public async Task UpdateDisplayConfigurationAsync(long id, DisplayConfigurationDto displayConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var validationResult = await _DisplayConfigurationValidator.ValidateAsync(displayConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<DisplayConfiguration>(displayConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.DisplayConfiguration, entity)
            });
    }

    public async Task UpdateKernelConfigurationAsync(long id, KernelConfigurationDto kernelConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var validationResult = await _KernelConfigurationValidator.ValidateAsync(kernelConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<KernelConfiguration>(kernelConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.KernelConfiguration, entity)
            });
    }

    public async Task UpdateTerminalConfigurationAsync(long id, TerminalConfigurationDto terminalConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var validationErrors = await _TerminalConfigurationValidator.ValidateAsync(terminalConfiguration);

        if (validationErrors.Errors.Any())
            throw new ModelValidationException(validationErrors.Errors);

        var entity = _Mapper.Map<TerminalConfiguration>(terminalConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.TerminalConfiguration, entity)
            });
    }

    public async Task UpdateProximityCouplingDeviceConfigurationAsync(long id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var validationResult = await _ProximityCouplingDeviceValidator.ValidateAsync(proximityCouplingDeviceConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<ProximityCouplingDeviceConfiguration>(proximityCouplingDeviceConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.ProximityCouplingDeviceConfiguration, entity)
            });
    }

    public async Task UpdateCertificateAuthorityConfigurationAsync(long id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PoSConfiguration), id);

        var validationResult = await _CertificateAuthorityConfigurationValidator.ValidateAsync(certificateAuthorityConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(System.Linq.Expressions.Expression<Func<PoSConfiguration, object>>, object)>
            {
                (item => item.CertificateAuthorityConfiguration, entity)
            });
    }

    public async Task<PoSConfigurationDto> GetTerminalPoSConfigurationAsync(long terminalId)
    {
        PoSConfiguration? configuration = await _PosRepository.FindByTerminalIdAsync(terminalId);

        return _Mapper.Map<PoSConfigurationDto>(configuration);
    }

    public async Task<PoSConfigurationDto> GetPoSConfigurationAsync(long id)
    {
        PoSConfiguration? configuration = await _PosRepository.FindByIdAsync(id);

        return _Mapper.Map<PoSConfigurationDto>(configuration);
    }

    public async Task<IEnumerable<PoSConfigurationDto>> GetStorePoSConfigurationsAsync(long storeId)
    {
        IEnumerable<PoSConfiguration> result = await _PosRepository.SelectPosConfigurationsByStoreIdAsync(storeId);

        return _Mapper.Map<IEnumerable<PoSConfigurationDto>>(result);
    }

    public async Task<IEnumerable<PoSConfigurationDto>> GetMerchantPoSConfigurationsAsync(long merchantId)
    {
        IEnumerable<PoSConfiguration> result = await _PosRepository.SelectPoSConfigurationsByMerchantIdAsync(merchantId);

        return _Mapper.Map<IEnumerable<PoSConfigurationDto>>(result);
    }
}
