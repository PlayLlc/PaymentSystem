using System.Linq;

using Play.Emv.Exceptions;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Messaging;
using Play.Emv.Sessions;

namespace Play.Emv.Kernel2.StateMachine;

public abstract class CommonProcessing
{
    #region Instance Values

    protected abstract StateId[] _ValidStateIds { get; }

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