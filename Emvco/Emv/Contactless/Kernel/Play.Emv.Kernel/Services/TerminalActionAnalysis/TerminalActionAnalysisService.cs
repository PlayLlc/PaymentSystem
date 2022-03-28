using System;

using Play.Ber.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;

namespace Play.Emv.Kernel.Services;

/// <remarks>
///     Book 3 Section 10.7
/// </remarks>
public class TerminalActionAnalysisService : IPerformTerminalActionAnalysis
{
    #region Instance Values

    private readonly IResolveAuthenticationType _AuthenticationTypeResolver;

    #endregion

    #region Instance Members

    public TerminalActionAnalysisService(IResolveAuthenticationType authenticationTypeResolver)
    {
        _AuthenticationTypeResolver = authenticationTypeResolver;
    }

    /// <summary>
    ///     Process
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    public GenerateApplicationCryptogramRequest Process(TransactionSessionId sessionId, KernelDatabase database)
    {
        ActionFlag resultFlag = ActionFlag.None;
        TerminalVerificationResults terminalVerificationResults =
            database.Get<TerminalVerificationResults>(TerminalVerificationResults.Tag);

        ProcessDenialActionCodes(database, terminalVerificationResults, ref resultFlag);
        ProcessOnlineActionCodes(database, terminalVerificationResults, ref resultFlag);
        ProcessDefaultActionCodes(database, terminalVerificationResults, ref resultFlag);

        if (resultFlag.HasFlag(ActionFlag.Denial))
            return CreateDenyTransactionResponse(sessionId, database);

        if (resultFlag.HasFlag(ActionFlag.Online))
            return CreateProceedOnlineResponse(sessionId, database);

        if (resultFlag.HasFlag(ActionFlag.Offline))
            return CreateProceedOfflineResponse(sessionId, database);

        throw new InvalidOperationException("The Terminal Action Analysis result could not be determined");
    }

    #endregion

    #region Denial

    /// <exception cref="TerminalDataException"></exception>
    private static ActionCodes GetDenialActionCodes(KernelDatabase database)
    {
        TerminalActionCodeDenial terminalActionCodeDenial = database.Get<TerminalActionCodeDenial>(TerminalActionCodeDenial.Tag);
        IssuerActionCodeDenial issuerActionCodeDefault = database.Get<IssuerActionCodeDenial>(IssuerActionCodeDenial.Tag);

        ActionCodes denialActionCodes =
            new((ulong) terminalActionCodeDenial.AsActionCodes() | (ulong) issuerActionCodeDefault.AsActionCodes());

        return denialActionCodes;
    }

    /// <summary>
    ///     Specifies the conditions that cause denial of a transaction without attempting to go online
    /// </summary>
    /// <param name="database"></param>
    /// <param name="terminalVerificationResults"></param>
    /// <param name="flag"></param>
    /// <exception cref="TerminalDataException"></exception>
    private static void ProcessDenialActionCodes(
        KernelDatabase database, TerminalVerificationResults terminalVerificationResults, ref ActionFlag flag)
    {
        if (!((ulong) terminalVerificationResults).AreAnyBitsSet((ulong) GetDenialActionCodes(database)))
            return;

        flag |= ActionFlag.Denial;
    }

    #endregion

    #region Default

    /// <exception cref="TerminalDataException"></exception>
    private static ActionCodes GetDefaultActionCodes(KernelDatabase database)
    {
        TerminalActionCodeDefault terminalActionCodeDenial = database.Get<TerminalActionCodeDefault>(TerminalActionCodeDefault.Tag);
        IssuerActionCodeDefault issuerActionCodeDefault = database.Get<IssuerActionCodeDefault>(IssuerActionCodeDefault.Tag);

        ActionCodes denialActionCodes =
            new((ulong) terminalActionCodeDenial.AsActionCodes() | (ulong) issuerActionCodeDefault.AsActionCodes());

        return denialActionCodes;
    }

    /// <summary>
    ///     specify the conditions that cause the transaction to be rejected if it might have been approved online but the
    ///     terminal is for any reason unable to process the transaction online
    /// </summary>
    /// <exception cref="TerminalDataException"></exception>
    private void ProcessDefaultActionCodes(
        KernelDatabase database, TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        ActionCodes defaultActionCodes = GetDefaultActionCodes(database);
        OutcomeParameterSet outcomeParameterSet = database.GetOutcomeParameterSet();

        if (database.IsTerminalType(TerminalType.CommunicationType.OfflineOnly))
            ProcessDefaultActionCodesForOfflineOnlyTerminals(defaultActionCodes, terminalVerificationResult, ref flag);
        else
        {
            ProcessDefaultActionCodesForOnlineCapableTerminals(outcomeParameterSet, defaultActionCodes, terminalVerificationResult,
                                                               ref flag);
        }
    }

    private void ProcessDefaultActionCodesForOfflineOnlyTerminals(
        ActionCodes defaultActionCodes, TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) defaultActionCodes))
            flag |= ActionFlag.Denial;
    }

    private void ProcessDefaultActionCodesForOnlineCapableTerminals(
        OutcomeParameterSet outcomeParameterSet, ActionCodes defaultActionCodes, TerminalVerificationResults terminalVerificationResult,
        ref ActionFlag flag)
    {
        if (!outcomeParameterSet.IsTimeout())
            return;

        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) defaultActionCodes))
            flag |= ActionFlag.Denial;
    }

    #endregion

    #region Online

    /// <exception cref="TerminalDataException"></exception>
    private static ActionCodes GetOnlineActionCodes(KernelDatabase database)
    {
        TerminalActionCodeOnline terminalActionCodeOnline = database.Get<TerminalActionCodeOnline>(TerminalActionCodeOnline.Tag);
        IssuerActionCodeOnline issuerActionCodeOnline = database.Get<IssuerActionCodeOnline>(IssuerActionCodeOnline.Tag);

        ActionCodes denialActionCodes =
            new((ulong) terminalActionCodeOnline.AsActionCodes() | (ulong) issuerActionCodeOnline.AsActionCodes());

        return denialActionCodes;
    }

    /// <exception cref="TerminalDataException"></exception>
    private void ProcessOnlineActionCodes(
        KernelDatabase database, TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        ActionCodes actionCodes = GetOnlineActionCodes(database);

        if (database.IsTerminalType(TerminalType.CommunicationType.OfflineOnly))
            ProcessOnlineActionCodesForOfflineOnlyTerminals(ref flag);
        else if (database.IsTerminalType(TerminalType.CommunicationType.OnlineAndOfflineCapable))
            ProcessOnlineActionCodesForOnlineAndOfflineCapableTerminals(actionCodes, terminalVerificationResult, ref flag);
        else
            flag |= ActionFlag.Online;
    }

    private void ProcessOnlineActionCodesForOnlineAndOfflineCapableTerminals(
        ActionCodes onlineActionCodes, TerminalVerificationResults terminalVerificationResult, ref ActionFlag flag)
    {
        if (((ulong) terminalVerificationResult).AreAnyBitsSet((ulong) onlineActionCodes))
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

    #region CAPDU Commands

    /// <param name="sessionId"></param>
    /// <param name="database"></param>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    private GenerateApplicationCryptogramRequest CreateDenyTransactionResponse(TransactionSessionId sessionId, KernelDatabase database)
    {
        CardRiskManagementDataObjectList1 cdol1 = database.Get<CardRiskManagementDataObjectList1>(CardRiskManagementDataObjectList1.Tag);
        DataStorageDataObjectList ddol = database.Get<DataStorageDataObjectList>(DataStorageDataObjectList.Tag);

        return GenerateApplicationCryptogramRequest.Create(sessionId,
                                                           new CryptogramInformationData(CryptogramTypes
                                                                                             .ApplicationAuthenticationCryptogram),
                                                           cdol1.AsDataObjectListResult(database), ddol.AsDataObjectListResult(database));
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    private GenerateApplicationCryptogramRequest CreateProceedOfflineResponse(TransactionSessionId sessionId, KernelDatabase database)
    {
        bool isCdaRequested =
            _AuthenticationTypeResolver.GetAuthenticationMethod(database.Get<TerminalCapabilities>(TerminalCapabilities.Tag),
                                                                database.Get<ApplicationInterchangeProfile>(ApplicationInterchangeProfile
                                                                    .Tag))
            == AuthenticationTypes.CombinedDataAuthentication;

        CardRiskManagementDataObjectList1 cdol1 = database.Get<CardRiskManagementDataObjectList1>(CardRiskManagementDataObjectList1.Tag);
        DataStorageDataObjectList ddol = database.Get<DataStorageDataObjectList>(DataStorageDataObjectList.Tag);

        return GenerateApplicationCryptogramRequest.Create(sessionId,
                                                           new CryptogramInformationData(CryptogramTypes.TransactionCryptogram,
                                                                                         isCdaRequested),
                                                           cdol1.AsDataObjectListResult(database), ddol.AsDataObjectListResult(database));
    }

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    /// <exception cref="OverflowException"></exception>
    private GenerateApplicationCryptogramRequest CreateProceedOnlineResponse(TransactionSessionId sessionId, KernelDatabase database)
    {
        CardRiskManagementDataObjectList1 cdol1 = database.Get<CardRiskManagementDataObjectList1>(CardRiskManagementDataObjectList1.Tag);
        DataStorageDataObjectList ddol = database.Get<DataStorageDataObjectList>(DataStorageDataObjectList.Tag);

        return GenerateApplicationCryptogramRequest.Create(sessionId,
                                                           new CryptogramInformationData(CryptogramTypes.AuthorizationRequestCryptogram),
                                                           cdol1.AsDataObjectListResult(database), ddol.AsDataObjectListResult(database));
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