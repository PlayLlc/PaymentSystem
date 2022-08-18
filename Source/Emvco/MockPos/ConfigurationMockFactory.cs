using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Configuration;
using Play.Emv.Kernel.Services;
using Play.Emv.Reader;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;

namespace MockPos
{
    public static class ConfigurationMockFactory
    {
        #region Terminal

        public static SystemTraceAuditNumberConfiguration CreateSystemTraceAuditNumberConfiguration() => throw new NotImplementedException();
        public static TerminalConfiguration CreateTerminalConfiguration() => throw new NotImplementedException();
        public static ISettleTransactions CreateSettler() => throw new NotImplementedException();

        #endregion

        #region Reader Main

        public static ReaderConfiguration CreateReaderConfiguration() => throw new NotImplementedException();
        public static KernelRetriever CreateKernelRetriever() => throw new NotImplementedException();

        #endregion
    }
}