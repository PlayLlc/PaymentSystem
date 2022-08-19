using System.Threading.Tasks;

using Play.Emv.Ber;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Kernel.Services;

public interface IManageTerminalRisk
{
    #region Instance Members

    public void Process(ITlvReaderAndWriter database, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount);

    #endregion
}