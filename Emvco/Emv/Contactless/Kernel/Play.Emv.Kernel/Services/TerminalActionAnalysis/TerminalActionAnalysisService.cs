using System;

using Play.Ber.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Configuration;
using Play.Emv.Icc;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Terminal.Contracts.Messages.Commands;

namespace Play.Emv.Kernel.Services;

/// <remarks>
///     Book 3 Section 10.7
/// </remarks>
public class TerminalActionAnalysisService : IPerformTerminalActionAnalysis
{
    #region Instance Values

    private readonly IHandlePcdRequests _PcdEndpoint;
    private readonly IResolveAuthenticationType _AuthenticationTypeResolver;
    private readonly TerminalCapabilities _TerminalCapabilities;
    private readonly TerminalType _TerminalType;
    private readonly ActionCodes _DefaultActionCodes;
    private readonly ActionCodes _OnlineActionCodes;
    private readonly ActionCodes _DenialActionCodes;

    #endregion

    #region Constructor

    public TerminalActionAnalysisService(
        IHandlePcdRequests pcdEndpoint,
        IResolveAuthenticationType authenticationTypeResolver,
        TerminalType terminalType,
        TerminalCapabilities terminalCapabilities,
        TerminalActionCodeDefault terminalActionCodeDefault,
        TerminalActionCodeDenial terminalActionCodeDenial,
        TerminalActionCodeOnline terminalActionCodeOnline,
        IssuerActionCodeDefault issuerActionCodeDefault,
        IssuerActionCodeDenial issuerActionCodeDenial,
        IssuerActionCodeOnline issuerActionCodeOnline)
    {
        _AuthenticationTypeResolver = authenticationTypeResolver;
        _TerminalCapabilities = terminalCapabilities;
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
    /// <exception cref="BerParsingException"></exception>
    public void Process(TerminalActionAnalysisCommand command)
    {
        ActionFlag resultFlag = ActionFlag.None;

        ProcessDenialActionCodes(command.GetTerminalVerificationResults(), ref resultFlag);
        ProcessOnlineActionCodes(command.GetTerminalVerificationResults(), ref resultFlag);
        ProcessDefaultActionCodes(command.GetTerminalVerificationResults(), command.GetOutcomeParameterSet(), ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Denial))
        {
            CreateDenyTransactionResponse(command);

            return;
        }

        if (resultFlag.HasFlag(ActionFlag.Online))
        {
            CreateProceedOnlineResponse(command);

            return;
        }

        if (resultFlag.HasFlag(ActionFlag.Offline))
        {
            CreateProceedOfflineResponse(command);

            return;
        }

        throw new InvalidOperationException("The Terminal Action Analysis result could not be determined");
    }

    #region Denial

    /// <summary>
    ///     Specifies the conditions that cause denial of a transaction without attempting to go online
    /// </summary>
    /// <param name="terminalVerificationResult"></param>
    /// <param name="flag"></param>
    private void ProcessDenialActionCodes(TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (!((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _DenialActionCodes))
            return;

        flag |= ActionFlag.Denial;
    }

    #endregion

    #endregion

    #region Online

    /// <param name="terminalVerificationResult"></param>
    /// <param name="flag"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void ProcessOnlineActionCodes(TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (_TerminalType.GetCommunicationType() == TerminalType.CommunicationType.OfflineOnly)
            ProcessOnlineActionCodesForOfflineOnlyTerminals(ref flag);
        else if (_TerminalType.GetCommunicationType() == TerminalType.CommunicationType.OnlineAndOfflineCapable)
            ProcessOnlineActionCodesForOnlineAndOfflineCapableTerminals(terminalVerificationResult, ref flag);
        else
            flag |= ActionFlag.Online;
    }

    private void ProcessOnlineActionCodesForOnlineAndOfflineCapableTerminals(
        TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _OnlineActionCodes))
            flag |= ActionFlag.Online;
        else
            flag |= ActionFlag.Offline;
    }

    private void ProcessOnlineActionCodesForOfflineOnlyTerminals(ref ActionFlag flag)
    {
        // Offline Only Terminals do not check the Online Action Codes. They will never go online
        flag |= ActionFlag.Offline;
    }

    #endregion

    #region Default

    /// <summary>
    ///     specify the conditions that cause the transaction to be rejected if it might have been approved online but the
    ///     terminal is for any reason unable to process the transaction online
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    private void ProcessDefaultActionCodes(
        TerminalVerificationResults terminalVerificationResult,
        OutcomeParameterSet outcomeParameterSet,
        ref ActionFlag flag)
    {
        if (_TerminalType.GetCommunicationType() != TerminalType.CommunicationType.OfflineOnly)
            ProcessDefaultActionCodesForOfflineOnlyTerminals(terminalVerificationResult, ref flag);
        else
            ProcessDefaultActionCodesForOnlineCapableTerminals(terminalVerificationResult, outcomeParameterSet, ref flag);
    }

    private void ProcessDefaultActionCodesForOfflineOnlyTerminals(
        TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _DefaultActionCodes))
            flag |= ActionFlag.Denial;
    }

    private void ProcessDefaultActionCodesForOnlineCapableTerminals(
        TerminalVerificationResults terminalVerificationResult,
        OutcomeParameterSet outcomeParameterSet,
        ref ActionFlag flag)
    {
        if (!outcomeParameterSet.IsTimeout())
            return;

        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) _DefaultActionCodes))
            flag |= ActionFlag.Denial;
    }

    #endregion

    #region CAPDU Commands

    /// <summary>
    ///     CreateDenyTransactionResponse
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void CreateDenyTransactionResponse(TerminalActionAnalysisCommand command)
    {
        _PcdEndpoint.Request(GenerateApplicationCryptogramRequest.Create(command.GetTransactionSessionId(),
                                                                         new CryptogramInformationData(CryptogramTypes
                                                                             .ApplicationAuthenticationCryptogram),
                                                                         command.GetCardRiskManagementDolResult(),
                                                                         command.GetDataStorageDolResult()));
    }

    /// <summary>
    ///     CreateProceedOfflineResponse
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void CreateProceedOfflineResponse(TerminalActionAnalysisCommand command)
    {
        bool isCdaRequested =
            _AuthenticationTypeResolver.GetAuthenticationMethod(_TerminalCapabilities, command.GetApplicationInterchangeProfile())
            == AuthenticationTypes.CombinedDataAuthentication;

        _PcdEndpoint.Request(GenerateApplicationCryptogramRequest.Create(command.GetTransactionSessionId(),
                                                                         new
                                                                             CryptogramInformationData(CryptogramTypes.TransactionCryptogram,
                                                                              isCdaRequested), command.GetCardRiskManagementDolResult(),
                                                                         command.GetDataStorageDolResult()));
    }

    /// <summary>
    ///     CreateProceedOnlineResponse
    /// </summary>
    /// <param name="command"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    private void CreateProceedOnlineResponse(TerminalActionAnalysisCommand command)
    {
        _PcdEndpoint.Request(GenerateApplicationCryptogramRequest.Create(command.GetTransactionSessionId(),
                                                                         new CryptogramInformationData(CryptogramTypes
                                                                             .AuthorizationRequestCryptogram),
                                                                         command.GetCardRiskManagementDolResult(),
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