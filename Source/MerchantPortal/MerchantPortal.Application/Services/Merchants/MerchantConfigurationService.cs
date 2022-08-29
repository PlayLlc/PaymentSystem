using AutoMapper;
using MerchantPortal.Application.Common.Exceptions;
using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Services.Merchants;

internal class MerchantConfigurationService : IMerchantConfigurationService
{
    private readonly IMerchantsRepository _merchantsRepository;
    private readonly IMapper _mapper;

    public MerchantConfigurationService(IMerchantsRepository merchantsRepository, IMapper mapper)
    {
        _merchantsRepository = merchantsRepository;
        _mapper = mapper;
    }

    public async Task<MerchantDto> GetMerchantAsync(long id)
    {
        return await Task.FromResult(_mapper.Map<MerchantDto>(_merchantsRepository.Query.FirstOrDefault(x => x.Id == id)));
    }

    public async Task<long> InsertMerchantAsync(MerchantDto merchant)
    {
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
