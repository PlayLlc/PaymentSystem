using AutoMapper;

using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Contracts.DTO;

using MerchantPortal.Core.Entities;

namespace Play.MerchantPortal.Application.Services.Terminals;

internal class TerminalConfigurationService : ITerminalConfigurationService
{
    #region Instance Values

    private readonly ITerminalsRepository _TerminalsRepository;
    private readonly IMapper _Mapper;

    #endregion

    #region Constructor

    public TerminalConfigurationService(ITerminalsRepository terminalsRepository, IMapper mapper)
    {
        _TerminalsRepository = terminalsRepository;
        _Mapper = mapper;
    }

    #endregion

    #region Instance Members

    public async Task DeleteTerminalAsync(long id)
    {
        _TerminalsRepository.DeleteEntity(new TerminalEntity {Id = id});

        await _TerminalsRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TerminalDto>> GetStoreTerminalsAsync(long storeId)
    {
        return await Task.FromResult(_Mapper.Map<IEnumerable<TerminalDto>>(_TerminalsRepository.SelectTerminalsByStore(storeId)));
    }

    public async Task<TerminalDto> GetTerminalAsync(long id)
    {
        TerminalEntity? entity = await _TerminalsRepository.SelectById(id);

        return _Mapper.Map<TerminalDto>(entity);
    }

    public async Task<long> InsertTerminalAsync(TerminalDto terminalDto)
    {
        var entity = _Mapper.Map<TerminalEntity>(terminalDto);

        _TerminalsRepository.AddEntity(entity);

        await _TerminalsRepository.SaveChangesAsync();

        //TODO: When a Merchant creates a new Terminal, the Terminal will create a default implementation of the terminal’s Point of Sale configuration. Separate pr.

        return entity.Id;
    }

    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateTerminalAsync(TerminalDto terminalDto)
    {
        var entity = _TerminalsRepository.Query.FirstOrDefault(x => x.Id == terminalDto.Id);

        if (entity == null)
            throw new NotFoundException(nameof(TerminalEntity), terminalDto.Id);

        UpdateEntity(entity, terminalDto);

        await _TerminalsRepository.SaveChangesAsync();
    }

    private void UpdateEntity(TerminalEntity entity, TerminalDto terminalDto)
    { }

    #endregion
}