using Play.Emv.Configuration;
using Play.Emv.Display.Configuration;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader;
using Play.Emv.Reader.Configuration;
using Play.Emv.Selection.Configuration;
using Play.Emv.Terminal.Session;

namespace MockPos
{
    public static class ConfigurationMockFactory
    {
        #region Instance Members

        public static SequenceConfiguration CreateSystemTraceAuditNumberConfiguration() => throw new NotImplementedException();
        public static TerminalConfiguration CreateTerminalConfiguration() => throw new NotImplementedException();
        public static SelectionConfiguration CreateSelectionConfiguration() => throw new NotImplementedException();
        public static ReaderDatabase CreateReaderConfiguration() => throw new NotImplementedException();
        public static DisplayConfigurations CreateDisplayConfiguration() => throw new NotImplementedException();
        public static PcdConfiguration CreatePcdProtocolConfiguration() => throw new NotImplementedException();

        #endregion
    }
}