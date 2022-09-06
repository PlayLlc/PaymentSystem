using AutoMapper;

using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Contracts.DTO;
using Play.MerchantPortal.Domain.Entities;
using FluentValidation;

namespace Play.MerchantPortal.Application.Services.Stores;

internal class StoreConfigurationService : IStoreConfigurationService
{
    #region Instance Values

    private readonly IStoresRepository _StoresRepository;
    private readonly IMapper _Mapper;
    private readonly IValidator<StoreDto> _Validator;

    #endregion

    #region Constructor

    public StoreConfigurationService(IStoresRepository storesRepository, IMapper mapper, IValidator<StoreDto> validator)
    {
        _StoresRepository = storesRepository;
        _Mapper = mapper;
        _Validator = validator;
    }

    #endregion

    #region Instance Members

    public async Task DeleteStoreAsync(long id)
    {
        _StoresRepository.DeleteEntity(new StoreEntity {Id = id});

        await _StoresRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<StoreDto>> GetMerchantStoresAsync(long merchantId)
    {
        var result = await _StoresRepository.SelectStoresByMerchant(merchantId);

        return _Mapper.Map<IEnumerable<StoreDto>>(result);
    }

    public async Task<StoreDto> GetStoreAsync(long id)
    {
        StoreEntity? entity = await _StoresRepository.SelectById(id);

        return _Mapper.Map<StoreDto>(entity);
    }

    public async Task<long> InsertStoreAsync(StoreDto storeDto)
    {
        var entity = _Mapper.Map<StoreEntity>(storeDto);

        _StoresRepository.AddEntity(entity);

        await _StoresRepository.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateStoreAsync(StoreDto storeDto)
    {
        var entity = _StoresRepository.Query.FirstOrDefault(x => x.Id == storeDto.Id);

        if (entity == null)
            throw new NotFoundException(nameof(StoreEntity), storeDto.Id);

        UpdateStoreEntity(entity, storeDto);

        await _StoresRepository.SaveChangesAsync();
    }

    private void UpdateStoreEntity(StoreEntity entity, StoreDto storeDto)
    {
        entity.Name = storeDto.Name;
        entity.Address = storeDto.Address;
    }

    #endregion
}