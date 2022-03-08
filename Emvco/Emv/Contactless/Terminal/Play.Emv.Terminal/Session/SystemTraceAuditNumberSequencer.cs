using System;

using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.DataElements;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Globalization.Time;

namespace Play.Emv.Terminal.Session;

internal class SystemTraceAuditNumberSequencer : IGenerateSequenceTraceAuditNumbers
{
    #region Static Metadata

    private const ushort _MinimumValue = 1;

    #endregion

    #region Instance Values

    private readonly uint _Threshold;
    private readonly IHandleTerminalRequests _TerminalEndpoint;
    private uint _Stan;

    #endregion

    #region Constructor

    public SystemTraceAuditNumberSequencer(SystemTraceAuditNumberConfiguration configuration, IHandleTerminalRequests terminalEndpoint)
    {
        _Threshold = configuration.Threshold;
        _Stan = configuration.SystemTraceAuditNumberInitializationValue;
        _TerminalEndpoint = terminalEndpoint;
    }

    #endregion

    #region Instance Members

    public SystemTraceAuditNumber Generate()
    {
        if (_Stan >= _Threshold)
            SendReconciliationMessage();

        _Stan++;

        return new SystemTraceAuditNumber(_Stan);
    }

    /// <summary>
    ///     Reset
    /// </summary>
    /// <param name="settlementAcknowledgement"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Reset(AcquirerResponseSignal settlementAcknowledgement)
    {
        if (settlementAcknowledgement.MessageTypeIndicator != MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse)
        {
            throw new InvalidOperationException(
                $"A process attempted to reset the {nameof(SystemTraceAuditNumberSequencer)} but the argument provided was not a {MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse.GetType().Name} response. The {nameof(MessageTypeIndicator)} provided was: [{settlementAcknowledgement.MessageTypeIndicator}] but should have been: [{new MessageTypeIndicator((ushort) MessageTypeIndicatorTypes.Reconciliation.ReconciliationResponse)}]");
        }

        _Stan = _MinimumValue;
    }

    // BUG: We're probably going to want to enqueue a signal in the Terminal Process, or maybe run this as a separate process that communicates independently with the Acquirer when the STAN reaches a threshold
    private void SendReconciliationMessage()
    {
        _TerminalEndpoint.Request(new InitiateSettlementRequest(DateTimeUtc.Now()));
    }

    #endregion

    public class Lock
    { }
}