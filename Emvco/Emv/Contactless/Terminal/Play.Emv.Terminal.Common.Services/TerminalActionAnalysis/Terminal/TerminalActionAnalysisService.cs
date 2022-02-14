using System;

using Play.Core.Extensions;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;

/// <remarks>
///     Book 3 Section 10.7
/// </remarks>
public class TerminalActionAnalysisService : IPerformTerminalActionAnalysis
{
    #region Instance Values

    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly TerminalType _TerminalType;
    private readonly ActionCodes _DefaultActionCodes;
    private readonly ActionCodes _OnlineActionCodes;
    private readonly ActionCodes _DenialActionCodes;

    #endregion

    #region Constructor

    public TerminalActionAnalysisService(
        IHandlePcdRequests pcdEndpoint,
        TerminalType terminalType,
        TerminalActionCodeDefault terminalActionCodeDefault,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalActionCodeOnline terminalActionCodeOnline,
        IssuerActionCodeDefault issuerActionCodeDefault,
        IssuerActionCodeDenial issuerActionCodeDenial,
        IssuerActionCodeOnline issuerActionCodeOnline)
    {
        _PcdEndpoint = pcdEndpoint;
        _TerminalType = terminalType;

        _DefaultActionCodes =
            new ActionCodes((ulong) terminalActionCodeDefault.AsActionCodes() | (ulong) issuerActionCodeDefault.AsActionCodes());
        _OnlineActionCodes =
            new ActionCodes((ulong) terminalActionCodeOnline.AsActionCodes() | (ulong) issuerActionCodeOnline.AsActionCodes());
        _DenialActionCodes =
            new ActionCodes((ulong) terminalActionCodeDenial.AsActionCodes() | (ulong) issuerActionCodeDenial.AsActionCodes());
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public void Process(TerminalActionAnalysisCommand command)
    {
        ActionFlag resultFlag = ActionFlag.None;

        ProcessDenialActionCodes(command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Offline))
        {
            CreateDenyTransactionResponse(command);

            return;
        }

        ProcessOnlineActionCodes(command.GetTerminalVerificationResults(), ref resultFlag);

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

        ProcessDefaultActionCodes(command.GetTerminalVerificationResults(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Denial))
        {
            CreateDenyTransactionResponse(command);

            return;
        }

        if (resultFlag.HasFlag(ActionFlag.Offline))
        {
            CreateProceedOfflineResponse(command);

            return;
        }

        throw new InvalidOperationException("The Terminal Action Analysis result could not be determined");
    }

    private void ProcessDenialActionCodes(TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (!((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _DenialActionCodes))
            return;

        flag |= ActionFlag.Denial;
    }

    /// <param name="terminalVerificationResult"></param>
    /// <param name="flag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void ProcessOnlineActionCodes(TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (_TerminalType.GetCommunicationType() == TerminalType.CommunicationType.OfflineOnly)
            return;

        if (_TerminalType.GetCommunicationType() == TerminalType.CommunicationType.OnlineOnly)
        {
            flag |= ActionFlag.Online;

            return;
        }

        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _OnlineActionCodes))
        {
            flag |= ActionFlag.Offline;

            return;
        }

        flag |= ActionFlag.Online;
    }

    /// <summary>
    ///     specify the conditions that cause the transaction to be rejected if it might have been approved online but the
    ///     terminal is for any reason unable to process the transaction online
    /// </summary>
    private void ProcessDefaultActionCodes(TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (_TerminalType == TerminalType.CommunicationType.OnlineOnly)
            return;

        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _DefaultActionCodes))
        {
            flag |= ActionFlag.Offline;

            return;
        }

        flag |= ActionFlag.Denial;
    }

    #endregion

    #region CAPDU Commands

    private void CreateDenyTransactionResponse(TerminalActionAnalysisCommand command) =>
        _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
            new CryptogramInformationData(CryptogramTypes.ApplicationAuthenticationCryptogram), command.GetCardRiskManagementDolResult(),
            command.GetDataStorageDolResult()));

    private void CreateProceedOfflineResponse(TerminalActionAnalysisCommand command)
    {
        _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
            new CryptogramInformationData(CryptogramTypes.TransactionCryptogram), command.GetCardRiskManagementDolResult(),
            command.GetDataStorageDolResult()));
    }

    private void CreateProceedOnlineResponse(TerminalActionAnalysisCommand command)
    {
        _PcdEndpoint.Request(GenerateApplicationCryptogramCommand.Create(command.GetTransactionSessionId(),
            new CryptogramInformationData(CryptogramTypes.AuthorizationRequestCryptogram), command.GetCardRiskManagementDolResult(),
            command.GetDataStorageDolResult()));
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