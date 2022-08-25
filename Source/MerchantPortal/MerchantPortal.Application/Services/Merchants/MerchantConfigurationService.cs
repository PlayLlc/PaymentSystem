using AutoMapper;
using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Core.Entities;

namespace MerchantPortal.Application.Services.Merchants;

internal class MerchantConfigurationService : IMerchantConfigurationService
{
    private readonly IMerchantsRepository _MerchantsRepository;
    private readonly IMapper _mapper;

    public MerchantConfigurationService(IMerchantsRepository merchantsRepository, IMapper mapper)
    {
        _MerchantsRepository = merchantsRepository;
        _mapper = mapper;
    }

    public async Task<MerchantDto> GetMerchantAsync(long id)
    {
        return await Task.FromResult(_mapper.Map<MerchantDto>(_MerchantsRepository.Query.FirstOrDefault(x => x.Id == id)));
    }

    public async Task InsertMerchantAsync(MerchantDto merchant)
    {
        var entity = _mapper.Map<MerchantEntity>(merchant);

        _MerchantsRepository.AddEntity(entity);
    }
}
