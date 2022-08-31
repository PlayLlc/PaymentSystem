using MerchantPortal.Application.Contracts.Persistence;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Core.Entities.PointOfSale;

namespace MerchantPortal.Application.Services.PoS;

internal class PoSConfigurationService : IPoSConfigurationService
{
    private readonly IPoSRepository _posRepository;

    public PoSConfigurationService(IPoSRepository posRepository)
    {
        _posRepository = posRepository;
    }

    public Task CreateNewPosConfiguration(CreatePosConfigurationDto initialConfiguration)
    {
        var entity = new PosConfigurationHeader()
        {
            CompanyId = initialConfiguration.CompanyId,
            MerchantId = initialConfiguration.MerchatId,
            StoreId = initialConfiguration.StoreId,
            TerminalId = initialConfiguration.TerminalId
        };

        _posRepository.Insert
    }

    public Task UpdateCertificateConfiguration()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePosCombinationConfiguration()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePosDisplayConfiguration()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePosKernelConfiguration()
    {
        throw new NotImplementedException();
    }

    public Task UpdatePosTerminalConfiguration(long terminalId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProximityCouplingDeviceConfiguration()
    {
        throw new NotImplementedException();
    }
}
