using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;

using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;

using Xunit.Sdk;

namespace Play.Emv.Terminal.Common.Tests;

public class TerminalActionAnalysisServiceFactory
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Constructor

    static TerminalActionAnalysisServiceFactory()
    { }

    #endregion

    #region Instance Members

    public static TerminalActionAnalysisService Create(TerminalType terminalType, IFixture fixture)
    {
        fixture.Register<TerminalActionAnalysisService>(() => new TerminalActionAnalysisService(fixture.Create<IHandlePcdRequests>(),
            terminalType, GetTerminalActionCodeDefault(), GetTerminalActionCodeDenial(), GetTerminalActionCodeOnline(),
            GetIssuerActionCodeDefault(), GetIssuerActionCodeDenial(), GetIssuerActionCodeOnline()));

        return fixture.Create<TerminalActionAnalysisService>();
    }

    #endregion

    #region Issuer Codes

    public static ActionCodes GetRandomIssuerActionCodeOnline() =>
        IssuerOnlineActionCodes.ElementAt(_Random.Next(0, IssuerDefaultActionCodes.Count - 1));

    public static ActionCodes GetRandomIssuerActionCodeDefault() =>
        IssuerDefaultActionCodes.ElementAt(_Random.Next(0, IssuerDefaultActionCodes.Count - 1));

    public static ActionCodes GetRandomIssuerActionCodeDenial() =>
        IssuerDenialActionCodes.ElementAt(_Random.Next(0, IssuerDenialActionCodes.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeOnline() =>
        TerminalOnlineActionCodes.ElementAt(_Random.Next(0, TerminalDefaultActionCodes.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeDefault() =>
        TerminalDefaultActionCodes.ElementAt(_Random.Next(0, TerminalDefaultActionCodes.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeDenial() =>
        TerminalDenialActionCodes.ElementAt(_Random.Next(0, TerminalDenialActionCodes.Count - 1));

    public static List<ActionCodes> IssuerOnlineActionCodes = new()
    {
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredPinPadPresentButPinWasNotEntered),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CardholderVerificationWasNotSuccessful)
    };

    public static List<ActionCodes> IssuerDenialActionCodes = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ExpiredApplication),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ApplicationNotYetEffective)
    };

    public static List<ActionCodes> IssuerDefaultActionCodes = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.IssuerAuthenticationFailed),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ScriptProcessingFailedAfterFinalGenerateAc),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ScriptProcessingFailedBeforeFinalGenerateAc)
    };

    #endregion

    #region Terminal Codes

    public static List<ActionCodes> TerminalOnlineActionCodes = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.MerchantForcedTransactionOnline),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.TransactionSelectedRandomlyForOnlineProcessing),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.TransactionExceedsFloorLimit),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.UpperConsecutiveOfflineLimitExceeded),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.UnrecognizedCvm)
    };

    public static List<ActionCodes> TerminalDenialActionCodes = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.IccDataMissing),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ExpiredApplication),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ApplicationNotYetEffective)
    };

    public static List<ActionCodes> TerminalDefaultActionCodes = new()
    {
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking)
    };

    #endregion

    #region Issuer Codes

    private static IssuerActionCodeOnline GetIssuerActionCodeOnline()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in IssuerOnlineActionCodes)
            result |= (ulong) actionCode;

        return new IssuerActionCodeOnline(result);
    }

    private static IssuerActionCodeDefault GetIssuerActionCodeDefault()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in IssuerDefaultActionCodes)
            result |= (ulong) actionCode;

        return new IssuerActionCodeDefault(result);
    }

    private static IssuerActionCodeDenial GetIssuerActionCodeDenial()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in IssuerDenialActionCodes)
            result |= (ulong) actionCode;

        return new IssuerActionCodeDenial(result);
    }

    #endregion

    #region Terminal Codes

    private static TerminalActionCodeDenial GetTerminalActionCodeDenial()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in TerminalDenialActionCodes)
            result |= (ulong) actionCode;

        return new TerminalActionCodeDenial(result);
    }

    private static TerminalActionCodeOnline GetTerminalActionCodeOnline()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in TerminalOnlineActionCodes)
            result |= (ulong) actionCode;

        return new TerminalActionCodeOnline(result);
    }

    private static TerminalActionCodeDefault GetTerminalActionCodeDefault()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in TerminalDefaultActionCodes)
            result |= (ulong) actionCode;

        return new TerminalActionCodeDefault(result);
    }

    #endregion

    #region Terminal Action Codes

    #endregion
}