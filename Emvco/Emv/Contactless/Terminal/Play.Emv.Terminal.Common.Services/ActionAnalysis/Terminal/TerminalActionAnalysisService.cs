using System;

using Play.Core.Extensions;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Terminal.Common.Services.ActionAnalysis.Terminal;

/// <remarks>
///     Book 3 Section 10.7
/// </remarks>
public class TerminalActionAnalysisService : IPerformTerminalActionAnalysis
{
    #region Instance Values

    private readonly TerminalActionCodeDefault _TerminalActionCodeDefault;
    private readonly TerminalActionCodeDenial _TerminalActionCodeDenial;
    private readonly TerminalActionCodeOnline _TerminalActionCodeOnline;
    private readonly TerminalType _TerminalType;

    #endregion

    #region Constructor

    public TerminalActionAnalysisService(
        TerminalActionCodeDefault terminalActionCodeDefault,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalActionCodeOnline terminalActionCodeOnline,
        TerminalType terminalType)
    {
        _TerminalActionCodeDefault = terminalActionCodeDefault;
        _TerminalActionCodeDenial = terminalActionCodeDenial;
        _TerminalActionCodeOnline = terminalActionCodeOnline;
        _TerminalType = terminalType;
    }

    #endregion

    #region Instance Members

    public TerminalActionAnalysisResponse CreateDenyTransactionResponse() => new(CryptogramType.ApplicationAuthenticationCryptogram);
    public TerminalActionAnalysisResponse CreateProceedOfflineResponse() => new(CryptogramType.TransactionCryptogram);
    public TerminalActionAnalysisResponse CreateProceedOnlineResponse() => new(CryptogramType.AuthorizationRequestCryptogram);

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TerminalActionAnalysisResponse Process(TerminalActionAnalysisCommand command)
    {
        ActionFlag resultFlag = ActionFlag.None;

        ProcessDenialActionCodes(_TerminalType, command.GetIssuerActionCodeDenial(), _TerminalActionCodeDenial,
                                 command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Denial))
            return CreateDenyTransactionResponse();

        ProcessOnlineActionCodes(_TerminalType, command.GetIssuerActionCodeOnline(), _TerminalActionCodeOnline,
                                 command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Offline))
            return CreateProceedOfflineResponse();

        if (resultFlag.HasFlag(ActionFlag.Online))
            return CreateProceedOnlineResponse();

        ProcessDefaultActionCodes(_TerminalType, command.GetIssuerActionCodeDefault(), _TerminalActionCodeDefault,
                                  command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Denial))
            return CreateDenyTransactionResponse();

        if (resultFlag.HasFlag(ActionFlag.Offline))
            return CreateProceedOnlineResponse();

        throw new InvalidOperationException("The Terminal Action Analysis result could not be determined");
    }

    /// <summary>
    ///     specify the conditions that cause the transaction to be rejected if it might have been approved online but the
    ///     terminal is for any
    ///     reason unable to process the transaction online
    /// </summary>
    private void ProcessDefaultActionCodes(
        TerminalType terminalType,
        ActionCodes issuerActionCodeDefault,
        TerminalActionCodeDefault terminalActionCodeDefault,
        TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        if (terminalType == TerminalType.CommunicationType.OnlineOnly)
            return;

        ActionCodes defaultActionCodes = new((ulong) issuerActionCodeDefault | (ulong) terminalActionCodeDefault.AsActionCodes());

        if (((ulong) terminalVerificationResult).AreBitsSet((ulong) defaultActionCodes))
        {
            flag |= ActionFlag.Offline;

            return;
        }

        flag |= ActionFlag.Denial;
    }

    private void ProcessDenialActionCodes(
        TerminalType terminalType,
        ActionCodes issuerActionCodeDenial,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        ActionCodes denialActionCodes = new((ulong) issuerActionCodeDenial | (ulong) terminalActionCodeDenial.AsActionCodes());
        if (((ulong) terminalVerificationResult).AreBitsSet((ulong) denialActionCodes))
            flag |= ActionFlag.Denial;
    }

    /// <summary>
    ///     ProcessOnlineActionCodes
    /// </summary>
    /// <param name="terminalType"></param>
    /// <param name="issuerActionCodeOnline"></param>
    /// <param name="terminalActionCodeOnline"></param>
    /// <param name="terminalVerificationResult"></param>
    /// <param name="flag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void ProcessOnlineActionCodes(
        TerminalType terminalType,
        ActionCodes issuerActionCodeOnline,
        TerminalActionCodeOnline terminalActionCodeOnline,
        TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        if (terminalType.GetCommunicationType() == TerminalType.CommunicationType.OfflineOnly)
            return;

        if (terminalType.GetCommunicationType() == TerminalType.CommunicationType.OnlineOnly)
        {
            flag |= ActionFlag.Online;

            return;
        }

        ActionCodes onlineActionCodes = new((ulong) issuerActionCodeOnline | (ulong) terminalActionCodeOnline.AsActionCodes());

        if (((ulong) terminalVerificationResult).AreBitsSet((ulong) onlineActionCodes))
        {
            flag |= ActionFlag.Offline;

            return;
        }

        flag |= ActionFlag.Online;
    }

    #endregion

    [Flags]
    public enum ActionFlag
    {
        None = 0,
        Offline = 1,
        Online = 2,
        Denial = 4
    }
}