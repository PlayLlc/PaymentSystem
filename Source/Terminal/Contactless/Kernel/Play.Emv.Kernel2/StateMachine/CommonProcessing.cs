using System.Linq;

using Play.Emv.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.DataExchange;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Messaging;

namespace Play.Emv.Kernel2.StateMachine;

public abstract class CommonProcessing
{
    #region Instance Values

    protected readonly KernelDatabase _Database;
    protected readonly DataExchangeKernelService _DataExchangeKernelService;
    protected readonly IGetKernelState _KernelStateResolver;
    protected readonly IEndpointClient _EndpointClient;
    protected abstract StateId[] _ValidStateIds { get; }

    #endregion

    #region Constructor

    protected CommonProcessing(
        KernelDatabase database, DataExchangeKernelService dataExchangeKernelService, IGetKernelState kernelStateResolver, IEndpointClient endpointClient)
    {
        _Database = database;
        _DataExchangeKernelService = dataExchangeKernelService;
        _KernelStateResolver = kernelStateResolver;
        _EndpointClient = endpointClient;
    }

    #endregion

    #region Instance Members

    /// <exception cref="RequestOutOfSyncException"></exception>
    protected void HandleRequestOutOfSync(StateId stateId)
    {
        // TODO: Update the Outcome parameters and send a 'STOP' signal to the Kernel
        if (_ValidStateIds.All(a => a != stateId))

        {
            throw new RequestOutOfSyncException(
                $" The request is invalid for the current state of the [{nameof(KernelChannel)}] channel. The {GetType().Name} can not process a request originating from the state with the {nameof(StateId)}: [{stateId}] ");
        }
    }

    public abstract StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message);

    #endregion
}