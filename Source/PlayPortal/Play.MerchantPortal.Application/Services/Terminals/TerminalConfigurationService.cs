﻿using AutoMapper;
using Play.MerchantPortal.Application.Common.Exceptions;
using Play.MerchantPortal.Application.Contracts.Persistence;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Contracts.DTO;
using MerchantPortal.Core.Entities;

namespace Play.MerchantPortal.Application.Services.Terminals;

internal class TerminalConfigurationService : ITerminalConfigurationService
{
    private readonly ITerminalsRepository _terminalsRepository;
    private readonly IMapper _mapper;

    public TerminalConfigurationService(ITerminalsRepository terminalsRepository, IMapper mapper)
    {
        _terminalsRepository = terminalsRepository;
        _mapper = mapper;
    }

    public async Task DeleteTerminalAsync(long id)
    {
        _terminalsRepository.DeleteEntity(new TerminalEntity { Id = id });

        await _terminalsRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<TerminalDto>> GetStoreTerminalsAsync(long storeId)
    {
        return await Task.FromResult(_mapper.Map<IEnumerable<TerminalDto>>(_terminalsRepository.SelectTerminalsByStore(storeId)));
    }

    public async Task<TerminalDto> GetTerminalAsync(long id)
    {
        TerminalEntity? entity = await _terminalsRepository.SelectById(id);

        return _mapper.Map<TerminalDto>(entity);
    }

    public async Task<long> InsertTerminalAsync(TerminalDto terminalDto)
    {
        var entity = _mapper.Map<TerminalEntity>(terminalDto);

        _terminalsRepository.AddEntity(entity);

        await _terminalsRepository.SaveChangesAsync();

        //TODO: When a Merchant creates a new Terminal, the Terminal will create a default implementation of the terminal’s Point of Sale configuration. Separate pr.

        return entity.Id;
    }

    public async Task UpdateTerminalAsync(TerminalDto terminalDto)
    {
        var entity = _terminalsRepository.Query.FirstOrDefault(x => x.Id == terminalDto.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TerminalEntity), terminalDto.Id);
        }

        UpdateEntity(entity, terminalDto);

        await _terminalsRepository.SaveChangesAsync();
    }

    private void UpdateEntity(TerminalEntity entity, TerminalDto terminalDto)
    {}
}
