using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Emv.Ber.DataElements;

namespace _TempConfiguration.Reader
{
    internal class ReaderConfiguration
    {
        #region Instance Values

        private readonly SystemTraceAuditNumberConfiguration _SystemTraceAuditNumberConfiguration;
        private readonly ReaderContactlessTransactionLimit _ReaderContactlessTransactionLimit;
        private readonly TerminalCapabilities _TerminalCapabilities;
        private readonly TerminalType _TerminalType;
        private readonly ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice _ReaderContactlessTransactionLimitWhenCvmIsNotOnDevice;
        private readonly ReaderContactlessTransactionLimitWhenCvmIsOnDevice _ReaderContactlessTransactionLimitWhenCvmIsOnDevice;
        private readonly ReaderCvmRequiredLimit _ReaderCvmRequiredLimit;

        #endregion
    }
}