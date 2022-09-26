using AutoMapper;

using FluentValidation;

using Play.Merchants.Application.Exceptions;
using Play.Merchants.Contracts.DTO;
using Play.Merchants.Contracts.Services;
using Play.Merchants.Domain.Entities;
using Play.Merchants.Domain.Repositories;

namespace Play.Merchants.Application.Services;

internal class TerminalConfigurationService : ITerminalConfigurationService
{
    #region Instance Values

    private readonly ITerminalsRepository _TerminalsRepository;
    private readonly IMapper _Mapper;
    private readonly IValidator<TerminalDto> _Validator;

    #endregion

    #region Constructor

    public TerminalConfigurationService(ITerminalsRepository terminalsRepository, IMapper mapper, IValidator<TerminalDto> validator)
    {
        _TerminalsRepository = terminalsRepository;
        _Mapper = mapper;
        _Validator = validator;
    }

    #endregion

    #region Instance Members

    public async Task DeleteTerminalAsync(long id)
    {
        _TerminalsRepository.DeleteEntity(new Terminal {Id = id});

        await _TerminalsRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TerminalDto>> GetStoreTerminalsAsync(long storeId)
    {
        IEnumerable<Terminal> result = await _TerminalsRepository.SelectTerminalsByStore(storeId);

        return _Mapper.Map<IEnumerable<TerminalDto>>(result);
    }

    public async Task<TerminalDto> GetTerminalAsync(long id)
    {
        Terminal? entity = await _TerminalsRepository.SelectById(id);

        return _Mapper.Map<TerminalDto>(entity);
    }

    public async Task<long> InsertTerminalAsync(TerminalDto terminalDto)
    {
        Terminal? entity = _Mapper.Map<Terminal>(terminalDto);

        _TerminalsRepository.AddEntity(entity);

        await _TerminalsRepository.SaveChangesAsync();

        //TODO: When a Merchant creates a new Terminal, the Terminal will create a default implementation of the terminal’s Point of Sale configuration. Separate pr.

        return entity.Id;
    }

    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateTerminalAsync(TerminalDto terminalDto)
    {
        Terminal? entity = _TerminalsRepository.Query.FirstOrDefault(x => x.Id == terminalDto.Id);

        if (entity == null)
            throw new NotFoundException(nameof(Terminal), terminalDto.Id);

        await _TerminalsRepository.SaveChangesAsync();
    }

    #endregion
}