﻿using System;
using System.Collections.Generic;
using System.Linq;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

public class TerminalActionAnalysisServiceFactory
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Constructor

    static TerminalActionAnalysisServiceFactory()
    {
        IssuerActionCodesDenial = GetIssuerActionCodesDenial();
        IssuerActionCodesOnline = GetIssuerActionCodesOnline();
        IssuerActionCodesDefault = GetIssuerActionCodesDefault();

        TerminalActionCodesDenial = GetTerminalActionCodesDenial();
        TerminalActionCodesOnline = GetTerminalActionCodesOnline();
        TerminalActionCodesDefault = GetTerminalActionCodesDefault();
    }

    #endregion

    #region Instance Members

    public static List<ActionCodes> GetIssuerActionCodesDenial() =>
        new()
        {
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ExpiredApplication),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ApplicationNotYetEffective)
        };

    public static List<ActionCodes> GetIssuerActionCodesOnline() =>
        new()
        {
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredPinPadPresentButPinWasNotEntered),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CardholderVerificationWasNotSuccessful),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
        };

    public static List<ActionCodes> GetIssuerActionCodesDefault() =>
        new()
        {
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.IssuerAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ScriptProcessingFailedAfterFinalGenerateAc),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ScriptProcessingFailedBeforeFinalGenerateAc),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
        };

    public static List<ActionCodes> GetTerminalActionCodesDenial() =>
        new()
        {
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.IccDataMissing),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ExpiredApplication),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ApplicationNotYetEffective)
        };

    public static List<ActionCodes> GetTerminalActionCodesOnline() =>
        new()
        {
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.MerchantForcedTransactionOnline),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.TransactionSelectedRandomlyForOnlineProcessing),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.TransactionExceedsFloorLimit),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.UpperConsecutiveOfflineLimitExceeded),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.UnrecognizedCvm),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
        };

    public static List<ActionCodes> GetTerminalActionCodesDefault() =>
        new()
        {
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
            new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
        };

    public static ActionCodes GetRandomIssuerActionCodeOnline() => IssuerActionCodesOnline.ElementAt(_Random.Next(0, IssuerActionCodesOnline.Count - 1));
    public static ActionCodes GetRandomIssuerActionCodeDefault() => IssuerActionCodesDefault.ElementAt(_Random.Next(0, IssuerActionCodesDefault.Count - 1));
    public static ActionCodes GetRandomIssuerActionCodeDenial() => IssuerActionCodesDenial.ElementAt(_Random.Next(0, IssuerActionCodesDenial.Count - 1));
    public static ActionCodes GetRandomTerminalActionCodeOnline() => TerminalActionCodesOnline.ElementAt(_Random.Next(0, TerminalActionCodesOnline.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeDefault() =>
        TerminalActionCodesDefault.ElementAt(_Random.Next(0, TerminalActionCodesDefault.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeDenial() => TerminalActionCodesDenial.ElementAt(_Random.Next(0, TerminalActionCodesDenial.Count - 1));

    #endregion

    #region Issuer Codes

    public static readonly List<ActionCodes> IssuerActionCodesDenial;
    public static readonly List<ActionCodes> IssuerActionCodesOnline;
    public static readonly List<ActionCodes> IssuerActionCodesDefault;

    #endregion

    #region Terminal Codes

    public static List<ActionCodes> TerminalActionCodesDenial;
    public static List<ActionCodes> TerminalActionCodesOnline;
    public static List<ActionCodes> TerminalActionCodesDefault;

    #endregion

    #region Issuer Codes

    private static IssuerActionCodeOnline GetIssuerActionCodeOnline()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in IssuerActionCodesOnline)
            result |= (ulong) actionCode;

        return new IssuerActionCodeOnline(result);
    }

    private static IssuerActionCodeDefault GetIssuerActionCodeDefault()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in IssuerActionCodesDefault)
            result |= (ulong) actionCode;

        return new IssuerActionCodeDefault(result);
    }

    private static IssuerActionCodeDenial GetIssuerActionCodeDenial()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in IssuerActionCodesDenial)
            result |= (ulong) actionCode;

        return new IssuerActionCodeDenial(result);
    }

    #endregion

    #region Terminal Codes

    private static TerminalActionCodeDenial GetTerminalActionCodeDenial()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in TerminalActionCodesDenial)
            result |= (ulong) actionCode;

        return new TerminalActionCodeDenial(result);
    }

    private static TerminalActionCodeOnline GetTerminalActionCodeOnline()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in TerminalActionCodesOnline)
            result |= (ulong) actionCode;

        return new TerminalActionCodeOnline(result);
    }

    private static TerminalActionCodeDefault GetTerminalActionCodeDefault()
    {
        ulong result = 0;

        foreach (ActionCodes actionCode in TerminalActionCodesDefault)
            result |= (ulong) actionCode;

        return new TerminalActionCodeDefault(result);
    }

    #endregion
}