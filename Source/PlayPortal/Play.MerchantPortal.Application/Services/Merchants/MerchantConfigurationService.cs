using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Contracts.DTO;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Domain.Entities;
using Play.MerchantPortal.Domain.Persistence;
using ModelValidationException = Play.MerchantPortal.Application.Common.Exceptions.ModelValidationException;

namespace Play.MerchantPortal.Application.Services.Merchants;

internal class MerchantConfigurationService : IMerchantConfigurationService
{
    #region Instance Values

    private readonly IMerchantsRepository _MerchantsRepository;
    private readonly IMapper _Mapper;
    private readonly IValidator<MerchantDto> _Validator;

    #endregion

    #region Constructor

    public MerchantConfigurationService(IMerchantsRepository merchantsRepository, IMapper mapper, IValidator<MerchantDto> validator)
    {
        _MerchantsRepository = merchantsRepository;
        _Mapper = mapper;
        _Validator = validator;
    }

    #endregion

    #region Instance Members

    public async Task<MerchantDto> GetMerchantAsync(long id)
    {
        MerchantEntity? entity = await _MerchantsRepository.SelectById(id);

        return _Mapper.Map<MerchantDto>(entity);
    }

    public async Task<long> InsertMerchantAsync(MerchantDto merchant)
    {
        ValidationResult validationResult = await _Validator.ValidateAsync(merchant);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        var entity = _Mapper.Map<MerchantEntity>(merchant);

        _MerchantsRepository.AddEntity(entity);

        await _MerchantsRepository.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateMerchantAsync(MerchantDto merchant)
    {
        var entity = _MerchantsRepository.Query.FirstOrDefault(x => x.Id == merchant.Id);

        if (entity == null)
            throw new NotFoundException(nameof(MerchantEntity), merchant.Id);

        ValidationResult validationResult = await _Validator.ValidateAsync(merchant);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        UpdateEntity(entity, merchant);

        await _MerchantsRepository.SaveChangesAsync();
    }

    private static void UpdateEntity(MerchantEntity entity, MerchantDto merchantDto)
    {
        entity.Name = merchantDto.Name;
        entity.StreetAddress = merchantDto.StreetAddress;
        entity.City = merchantDto.City;
        entity.ZipCode = merchantDto.ZipCode;
        entity.State = merchantDto.State;
        entity.Country = merchantDto.Country;
    }

    #endregion
}