﻿using System.Linq;

using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Kernel2.StateMachine;

internal abstract class CommonProcessing
{
    #region Instance Values

    protected readonly KernelDatabase _KernelDatabase;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    protected readonly IGetKernelState _KernelStateResolver;
    protected readonly IHandlePcdRequests _PcdEndpoint;
    protected readonly IKernelEndpoint _KernelEndpoint;
    protected abstract StateId[] _ValidStateIds { get; }

    #endregion

    #region Constructor

    protected CommonProcessing(
        KernelDatabase kernelDatabase,
        DataExchangeKernelService dataExchangeKernelService,
        IGetKernelState kernelStateResolver,
        IHandlePcdRequests pcdEndpoint,
        IKernelEndpoint kernelEndpoint)
    {
        _KernelDatabase = kernelDatabase;
        _DataExchangeKernelService = dataExchangeKernelService;
        _KernelStateResolver = kernelStateResolver;
        _PcdEndpoint = pcdEndpoint;
        _KernelEndpoint = kernelEndpoint;
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected void HandleRequestOutOfSync(StateId stateId)
    {
        if (_ValidStateIds.All(a => a != stateId))

        {
            throw new
                RequestOutOfSyncException($" The request is invalid for the current state of the [{ChannelType.GetChannelTypeName(ChannelType.Kernel)}] channel. The {GetType().Name} can not process a request originating from the state with the {nameof(StateId)}: [{stateId}] ");
        }
    }

    public abstract KernelState Process(IGetKernelStateId kernelStateId, Kernel2Session session);

    #endregion
}