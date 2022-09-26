using System.Linq.Expressions;

using AutoMapper;

using FluentValidation;

using Play.Merchants.Application.Common.Exceptions;
using Play.Merchants.Contracts.Messages;
using Play.Merchants.Contracts.Messages.PointOfSale;
using Play.Merchants.Contracts.Services;
using Play.Merchants.Domain.Entities.PointOfSale;
using Play.Merchants.Domain.Repositories;

namespace Play.Merchants.Application.Services.PointsOfSale;

internal class PointOfSaleConfigurationService : IPointOfSaleConfigurationService
{
    #region Instance Values

    private readonly IPointOfSaleRepository _PosRepository;
    private readonly IMapper _Mapper;

    private readonly IValidator<TerminalConfigurationDto> _TerminalConfigurationValidator;
    private readonly IValidator<ProximityCouplingDeviceConfigurationDto> _ProximityCouplingDeviceValidator;
    private readonly IValidator<CertificateAuthorityConfigurationDto> _CertificateAuthorityConfigurationValidator;
    private readonly IValidator<DisplayConfigurationDto> _DisplayConfigurationValidator;
    private readonly IValidator<KernelConfigurationDto> _KernelConfigurationValidator;
    private readonly IValidator<CombinationConfigurationDto> _CombinationConfigurationValidator;

    #endregion

    #region Constructor

    public PointOfSaleConfigurationService(
        IPointOfSaleRepository posRepository, IMapper mapper, IValidator<TerminalConfigurationDto> terminalConfigurationValidator,
        IValidator<ProximityCouplingDeviceConfigurationDto> proximityCouplingDeviceValidator,
        IValidator<CertificateAuthorityConfigurationDto> certificateAuthorityConfigurationValidator,
        IValidator<DisplayConfigurationDto> displayConfigurationValidator, IValidator<KernelConfigurationDto> kernelConfigurationValidator,
        IValidator<CombinationConfigurationDto> combinationConfigurationValidator)
    {
        _PosRepository = posRepository;
        _Mapper = mapper;
        _TerminalConfigurationValidator = terminalConfigurationValidator;
        _ProximityCouplingDeviceValidator = proximityCouplingDeviceValidator;
        _CertificateAuthorityConfigurationValidator = certificateAuthorityConfigurationValidator;
        _DisplayConfigurationValidator = displayConfigurationValidator;
        _KernelConfigurationValidator = kernelConfigurationValidator;
        _CombinationConfigurationValidator = combinationConfigurationValidator;
    }

    #endregion

    #region Instance Members

    public async Task CreatePosConfigurationAsync(CreatePosConfigurationDto initialConfiguration)
    {
        var entity = new PointOfSaleConfiguration()
        {
            CompanyId = initialConfiguration.CompanyId, MerchantId = initialConfiguration.MerchantId, StoreId = initialConfiguration.StoreId,
            TerminalId = initialConfiguration.TerminalId
        };

        await _PosRepository.InsertNewPosConfigurationAsync(entity);
    }

    public async Task UpdateCombinationsConfigurationAsync(Guid id, IEnumerable<CombinationConfigurationDto> combinations)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PointOfSaleConfiguration), id);

        var collection = _Mapper.Map<IEnumerable<CombinationConfiguration>>(combinations);

        var validationErrors = combinations.SelectMany(combination => _CombinationConfigurationValidator.Validate(combination).Errors);

        if (validationErrors.Any())
            throw new ModelValidationException(validationErrors);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> {(item => item.Combinations, collection)});
    }

    public async Task UpdateDisplayConfigurationAsync(Guid id, DisplayConfigurationDto displayConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PointOfSaleConfiguration), id);

        var validationResult = await _DisplayConfigurationValidator.ValidateAsync(displayConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<DisplayConfiguration>(displayConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> {(item => item.DisplayConfiguration, entity)});
    }

    public async Task UpdateKernelConfigurationAsync(Guid id, KernelConfigurationDto kernelConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PointOfSaleConfiguration), id);

        var validationResult = await _KernelConfigurationValidator.ValidateAsync(kernelConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<KernelConfiguration>(kernelConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> {(item => item.KernelConfiguration, entity)});
    }

    public async Task UpdateTerminalConfigurationAsync(Guid id, TerminalConfigurationDto terminalConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PointOfSaleConfiguration), id);

        var validationResult = await _TerminalConfigurationValidator.ValidateAsync(terminalConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<TerminalConfiguration>(terminalConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> {(item => item.TerminalConfiguration, entity)});
    }

    public async Task UpdateProximityCouplingDeviceConfigurationAsync(Guid id, ProximityCouplingDeviceConfigurationDto proximityCouplingDeviceConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PointOfSaleConfiguration), id);

        var validationResult = await _ProximityCouplingDeviceValidator.ValidateAsync(proximityCouplingDeviceConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<ProximityCouplingDeviceConfiguration>(proximityCouplingDeviceConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> {(item => item.ProximityCouplingDeviceConfiguration, entity)});
    }

    public async Task UpdateCertificateAuthorityConfigurationAsync(Guid id, CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        bool exists = await _PosRepository.ExistsAsync(id);

        if (!exists)
            throw new NotFoundException(nameof(PointOfSaleConfiguration), id);

        var validationResult = await _CertificateAuthorityConfigurationValidator.ValidateAsync(certificateAuthorityConfiguration);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<CertificateAuthorityConfiguration>(certificateAuthorityConfiguration);

        await _PosRepository.UpdateGivenFieldsAsync(id,
            new List<(Expression<Func<PointOfSaleConfiguration, object>>, object)> {(item => item.CertificateAuthorityConfiguration, entity)});
    }

    public async Task<PointOfSaleConfigurationDto> GetTerminalConfigurationAsync(long terminalId)
    {
        PointOfSaleConfiguration? configuration = await _PosRepository.FindByTerminalIdAsync(terminalId);

        return _Mapper.Map<PointOfSaleConfigurationDto>(configuration);
    }

    public async Task<PointOfSaleConfigurationDto> GetPosConfigurationAsync(Guid id)
    {
        PointOfSaleConfiguration? configuration = await _PosRepository.FindByIdAsync(id);

        return _Mapper.Map<PointOfSaleConfigurationDto>(configuration);
    }

    public async Task<IEnumerable<PointOfSaleConfigurationDto>> GetPosConfigurationByStoreIdAsync(long storeId)
    {
        IEnumerable<PointOfSaleConfiguration> result = await _PosRepository.SelectPosConfigurationsByStoreIdAsync(storeId);

        return _Mapper.Map<IEnumerable<PointOfSaleConfigurationDto>>(result);
    }

    public async Task<IEnumerable<PointOfSaleConfigurationDto>> GetPosConfigurationByMerchantIdAsync(long merchantId)
    {
        IEnumerable<PointOfSaleConfiguration> result = await _PosRepository.SelectPoSConfigurationsByMerchantIdAsync(merchantId);

        return _Mapper.Map<IEnumerable<PointOfSaleConfigurationDto>>(result);
    }

    #endregion
}