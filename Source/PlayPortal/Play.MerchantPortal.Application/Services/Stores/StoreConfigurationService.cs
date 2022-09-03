using AutoMapper;
using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Contracts.DTO;
using MerchantPortal.Core.Entities;

namespace Play.MerchantPortal.Application.Services.Stores;

internal class StoreConfigurationService : IStoreConfigurationService
{
    private readonly IStoresRepository _storesRepository;
    private readonly IMapper _mapper;

    public StoreConfigurationService(IStoresRepository storesRepository, IMapper mapper)
    {
        _storesRepository = storesRepository;
        _mapper = mapper;
    }

    public async Task DeleteStoreAsync(long id)
    {
        _storesRepository.DeleteEntity(new StoreEntity { Id = id });

        await _storesRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<StoreDto>> GetMerchantStoresAsync(long merchantId)
    {
        return await Task.FromResult(_mapper.Map<IEnumerable<StoreDto>>(_storesRepository.SelectStoresByMerchant(merchantId)));
    }

    public async Task<StoreDto> GetStoreAsync(long id)
    {
        StoreEntity? entity = await _storesRepository.SelectById(id);

        return _mapper.Map<StoreDto>(entity);
    }

    public async Task<long> InsertStoreAsync(StoreDto storeDto)
    {
        var entity = _mapper.Map<StoreEntity>(storeDto);

        _storesRepository.AddEntity(entity);

        await _storesRepository.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateStoreAsync(StoreDto storeDto)
    {
        var entity = _storesRepository.Query.FirstOrDefault(x => x.Id == storeDto.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(StoreEntity), storeDto.Id);
        }

        UpdateStoreEntity(entity, storeDto);

        await _storesRepository.SaveChangesAsync();
    }

    private void UpdateStoreEntity(StoreEntity entity, StoreDto storeDto)
    {
        entity.Name = storeDto.Name;
        entity.Address = storeDto.Address;
    }
}
