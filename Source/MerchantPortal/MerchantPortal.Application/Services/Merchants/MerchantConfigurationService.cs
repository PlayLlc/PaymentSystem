﻿using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MerchantPortal.Application.Common.Exceptions;
using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Core.Entities;
using ValidationException = MerchantPortal.Application.Common.Exceptions.ValidationException;

namespace MerchantPortal.Application.Services.Merchants;

internal class MerchantConfigurationService : IMerchantConfigurationService
{
    private readonly IMerchantsRepository _merchantsRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<MerchantDto> _validator;

    public MerchantConfigurationService(IMerchantsRepository merchantsRepository, IMapper mapper, IValidator<MerchantDto> validator)
    {
        _merchantsRepository = merchantsRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<MerchantDto> GetMerchantAsync(long id)
    {
        MerchantEntity entity = await _merchantsRepository.SelectById(id);

        return _mapper.Map<MerchantDto>(entity);
    }

    public async Task<long> InsertMerchantAsync(MerchantDto merchant)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(merchant);

        if (validationResult.Errors.Any())
        {
            throw new ValidationException(validationResult.Errors);
        }

        var entity = _mapper.Map<MerchantEntity>(merchant);

        _merchantsRepository.AddEntity(entity);

        await _merchantsRepository.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateMerchantAsync(long id, MerchantDto merchant)
    {
        var entity = _merchantsRepository.Query.FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(MerchantEntity), id);
        }

        ValidationResult validationResult = await _validator.ValidateAsync(merchant);

        if (validationResult.Errors.Any())
        {
            throw new ValidationException(validationResult.Errors);
        }

        UpdateEntity(entity, merchant);

        await _merchantsRepository.SaveChangesAsync();
    }

    private void UpdateEntity(MerchantEntity entity, MerchantDto merchantDto)
    {
        entity.Name = merchantDto.Name;
        entity.StreetAddress = merchantDto.StreetAddress;
        entity.City = merchantDto.City;
        entity.ZipCode = merchantDto.ZipCode;
        entity.State = merchantDto.State;
        entity.Country = merchantDto.Country;
    }
}
