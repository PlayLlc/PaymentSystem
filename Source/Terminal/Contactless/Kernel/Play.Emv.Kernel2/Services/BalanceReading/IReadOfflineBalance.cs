using Play.Emv.Identifiers;
using Play.Emv.Kernel.State;
using Play.Emv.Kernel2.Databases;
using Play.Messaging;

namespace Play.Emv.Kernel2.Services.BalanceReading;

public interface IReadOfflineBalance
{
    #region Instance Members

    public StateId Process(IGetKernelStateId currentStateIdRetriever, Kernel2Session session, Message message);

    #endregion
}