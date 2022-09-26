using AutoMapper;

using FluentValidation;
using FluentValidation.Results;

using Play.Merchants.Application.Exceptions;
using Play.Merchants.Contracts.DTO;
using Play.Merchants.Contracts.Services;
using Play.Merchants.Domain.Entities;
using Play.Merchants.Domain.Repositories;

using ModelValidationException = Play.Merchants.Application.Exceptions.ModelValidationException;

namespace Play.Merchants.Application.Services;

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
        Merchant? entity = await _MerchantsRepository.SelectById(id);

        return _Mapper.Map<MerchantDto>(entity);
    }

    public async Task<long> InsertMerchantAsync(MerchantDto merchant)
    {
        ValidationResult validationResult = await _Validator.ValidateAsync(merchant);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        Merchant? entity = _Mapper.Map<Merchant>(merchant);

        _MerchantsRepository.AddEntity(entity);

        await _MerchantsRepository.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateMerchantAsync(MerchantDto merchant)
    {
        Merchant? entity = _MerchantsRepository.Query.FirstOrDefault(x => x.Id == merchant.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Merchant), merchant.Id);

        ValidationResult validationResult = await _Validator.ValidateAsync(merchant);

        if (validationResult.Errors.Any())
            throw new ModelValidationException(validationResult.Errors);

        UpdateEntity(entity, merchant);

        await _MerchantsRepository.SaveChangesAsync();
    }

    private static void UpdateEntity(Merchant entity, MerchantDto merchantDto)
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