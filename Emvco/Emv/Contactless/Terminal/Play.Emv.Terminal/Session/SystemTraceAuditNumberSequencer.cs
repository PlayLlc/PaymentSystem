using System;
using System.Threading;

using Play.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts;
using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Interchange.DataFields;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Common.Services.SequenceNumberManagement;

public class SystemTraceAuditNumberConfiguration
{
    #region Instance Values

    /// <summary>
    ///     This value determines the maximum value that the <see cref="SystemTraceAuditNumber" /> can reach before sending a
    ///     Settlement request to the Acquirer
    /// </summary>
    public uint Threshold;

    /// <summary>
    ///     This is the value of the last <see cref="SystemTraceAuditNumber" /> the Terminal held before shutting down
    /// </summary>
    public uint SystemTraceAuditNumberInitializationValue;

    #endregion
}

internal class SystemTraceAuditNumberSequencer : IGenerateSequenceTraceAuditNumbers
{
    #region Static Metadata

    private const ushort _MinimumValue = 1;

    #endregion

    #region Instance Values

    private readonly uint _Threshold;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private readonly Lock _Lock;

    #endregion

    #region Constructor

    public SystemTraceAuditNumberSequencer(SystemTraceAuditNumberConfiguration configuration, IHandleTerminalRequests terminalEndpoint)
    {
        _Threshold = configuration.Threshold;
        _Lock = new Lock {Stan = configuration.SystemTraceAuditNumberInitializationValue};
        _TerminalEndpoint = terminalEndpoint;
    }

    #endregion

    #region Instance Members

    public SystemTraceAuditNumber Generate()
    {
        lock (_Lock)
        {
            if (_Lock.Stan >= _Threshold)
                SendReconciliationMessage();

            _Lock.Stan++;

            return new SystemTraceAuditNumber(_Lock.Stan);
        }
    }

    public void Reset(AcquirerResponseSignal settlementAcknowledgement)
    {
        if (settlementAcknowledgement.MessageTypeIndicator != MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse)
        {
            throw new InvalidOperationException(
                $"A process attempted to reset the {nameof(SystemTraceAuditNumberSequencer)} but the argument provided was not a {MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse.GetType().Name} response. The {nameof(MessageTypeIndicator)} provided was: [{settlementAcknowledgement.MessageTypeIndicator}] but should have been: [{(MessageTypeIndicator) MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse}]");
        }

        lock (_Lock)
        {
            _Lock.Stan = _MinimumValue;
        }
    }

    // BUG: We're probably going to want to enqueue a signal in the Terminal Process, or maybe run this as a separate process that communicates independently with the Acquirer when the STAN reaches a threshold
    private void SendReconciliationMessage()
    {
        _TerminalEndpoint.Request(new InitiateSettlementRequest(DateTimeUtc.Now()));
    }

    #endregion

    public class Lock
    {
        #region Instance Values

        public uint Stan;

        #endregion
    }
}