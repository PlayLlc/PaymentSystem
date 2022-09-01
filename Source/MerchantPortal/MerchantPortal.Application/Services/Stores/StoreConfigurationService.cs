using AutoMapper;
using MerchantPortal.Application.Common.Exceptions;
using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Services.Stores;

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

    public async Task<IEnumerable<StoreHeaderDto>> GetMerchantStoresAsync(long merchantId)
    {
        return await Task.FromResult(_mapper.Map<IEnumerable<StoreHeaderDto>>(_storesRepository.SelectStoresByMerchant(merchantId)));
    }

    public async Task<StoreDto> GetStoreAsync(long id)
    {
        StoreEntity entity = await _storesRepository.SelectById(id);

        return _mapper.Map<StoreDto>(entity);
    }

    public async Task<long> InsertStoreAsync(StoreDto storeDto)
    {
        var entity = _mapper.Map<StoreEntity>(storeDto);

        _storesRepository.AddEntity(entity);

        await _storesRepository.SaveChangesAsync();

        return entity.Id;
    }

    public async Task UpdateStoreAsync(long id, StoreDto storeDto)
    {
        var entity = _storesRepository.Query.FirstOrDefault(x => x.Id == id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(StoreEntity), id);
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
