using Moq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

public class KernelDatabaseFactory
{
    #region Instance Members

    public static KernelDatabaseBuilder GetBuilder() => new();

    #endregion

    public class KernelDatabaseBuilder
    {
        #region Instance Values

        private readonly Mock<KernelDatabase> _Database = new();

        #endregion

        #region Instance Members

        public void SetOfflineOnlyTerminal()
        {
            _Database.Setup(a => a.Get(TerminalType.Tag)).Returns(new TerminalType(TerminalType.CommunicationType.OfflineOnly));
        }

        public void SetOnlineOnlyTerminal()
        {
            _Database.Setup(a => a.Get(TerminalType.Tag)).Returns(new TerminalType(TerminalType.CommunicationType.OnlineOnly));
        }

        public void SetOnlineAndOfflineCapableTerminal()
        {
            _Database.Setup(a => a.Get(TerminalType.Tag)).Returns(new TerminalType(TerminalType.CommunicationType.OnlineAndOfflineCapable));
        }

        public KernelDatabase Complete() => _Database.Object;

        #endregion
    }
}