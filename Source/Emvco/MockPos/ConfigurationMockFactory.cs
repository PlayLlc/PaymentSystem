using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Configuration;
using Play.Emv.Kernel.Services;
using Play.Emv.Reader;
using Play.Emv.Selection.Configuration;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;

namespace MockPos
{
    public static class ConfigurationMockFactory
    {
        public static SystemTraceAuditNumberConfiguration CreateSystemTraceAuditNumberConfiguration() => throw new NotImplementedException();
        public static TerminalConfiguration CreateTerminalConfiguration() => throw new NotImplementedException();
        public static SelectionConfiguration CreateSelectionConfiguration() => throw new NotImplementedException();
        public static ReaderConfiguration CreateReaderConfiguration() => throw new NotImplementedException();
    }
}