using System;
using System.Threading.Tasks;

using Play.Core.Extensions;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Icc.GenerateApplicationCryptogram;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Pcd.Contracts.SignalIn.Queriesdd;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;

/// <remarks>
///     Book 3 Section 10.7
/// </remarks>
public class TerminalActionAnalysisService : IPerformTerminalActionAnalysis
{
    #region Instance Values

    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly TerminalActionCodeDefault _TerminalActionCodeDefault;
    private readonly TerminalActionCodeDenial _TerminalActionCodeDenial;
    private readonly TerminalActionCodeOnline _TerminalActionCodeOnline;
    private readonly TerminalType _TerminalType;

    #endregion

    #region Constructor

    public TerminalActionAnalysisService(
        IHandlePcdRequests pcdEndpoint,
        TerminalActionCodeDefault terminalActionCodeDefault,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalActionCodeOnline terminalActionCodeOnline,
        TerminalType terminalType)
    {
        _PcdEndpoint = pcdEndpoint;
        _TerminalActionCodeDefault = terminalActionCodeDefault;
        _TerminalActionCodeDenial = terminalActionCodeDenial;
        _TerminalActionCodeOnline = terminalActionCodeOnline;
        _TerminalType = terminalType;
    }

    #endregion

    #region Instance Members

    public void CreateDenyTransactionResponse(TerminalActionAnalysisCommand command) =>
        _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
                                                                         new CryptogramInformationData(CryptogramType
                                                                             .ApplicationAuthenticationCryptogram),
                                                                         command.GetCardRiskManagementDolResult(),
                                                                         command.GetDataStorageDolResult()));

    public void CreateProceedOfflineResponse(TerminalActionAnalysisCommand command)
    {
        _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
                                                                         new CryptogramInformationData(CryptogramType
                                                                             .TransactionCryptogram),
                                                                         command.GetCardRiskManagementDolResult(),
                                                                         command.GetDataStorageDolResult()));
    }

    public void CreateProceedOnlineResponse(TerminalActionAnalysisCommand command)
    {
        _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
                                                                         new CryptogramInformationData(CryptogramType
                                                                             .AuthorizationRequestCryptogram),
                                                                         command.GetCardRiskManagementDolResult(),
                                                                         command.GetDataStorageDolResult()));
    }

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(TerminalActionAnalysisCommand command)
    {
        ActionFlag resultFlag = ActionFlag.None;

        if (ProcessDenialActionCodes(_TerminalType, command.GetIssuerActionCodeDenial(), _TerminalActionCodeDenial,
                                     command.GetTerminalVerificationResults(), ref resultFlag))
        {
            _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
                                                                             new CryptogramInformationData(CryptogramType
                                                                                 .ApplicationAuthenticationCryptogram),
                                                                             command.GetCardRiskManagementDolResult(),
                                                                             command.GetDataStorageDolResult()));
        }

        ProcessOnlineActionCodes(_TerminalType, command.GetIssuerActionCodeOnline(), _TerminalActionCodeOnline,
                                 command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Offline))
        {
            CreateProceedOfflineResponse(command);

            return;
        }

        if (resultFlag.HasFlag(ActionFlag.Online))
        {
            CreateProceedOnlineResponse(command);

            return;
        }

        ProcessDefaultActionCodes(_TerminalType, command.GetIssuerActionCodeDefault(), _TerminalActionCodeDefault,
                                  command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Denial))
        {
            CreateDenyTransactionResponse(command);

            return;
        }

        if (resultFlag.HasFlag(ActionFlag.Offline))
        {
            CreateProceedOnlineResponse(command);

            return;
        }

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

    private bool ProcessDenialActionCodes(
        TerminalType terminalType,
        ActionCodes issuerActionCodeDenial,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        ActionCodes denialActionCodes = new((ulong) issuerActionCodeDenial | (ulong) terminalActionCodeDenial.AsActionCodes());

        if (((ulong) terminalVerificationResult).AreBitsSet((ulong) denialActionCodes))
            return true;

        return false;
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