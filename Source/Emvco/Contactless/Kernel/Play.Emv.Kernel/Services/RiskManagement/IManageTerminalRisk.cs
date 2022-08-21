using Play.Emv.Ber;

namespace Play.Emv.Kernel.Services;

public interface IManageTerminalRisk
{
    #region Instance Members

    public void Process(ITlvReaderAndWriter database, ushort applicationTransactionCount, ushort lastOnlineApplicationTransactionCount);

    #endregion
}