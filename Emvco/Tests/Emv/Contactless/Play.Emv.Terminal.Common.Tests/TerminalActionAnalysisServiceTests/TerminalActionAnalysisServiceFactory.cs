using System;
using System.Collections.Generic;
using System.Linq;

using AutoFixture;

using Moq;

using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Security;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

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

    public static TerminalActionAnalysisService Create(TerminalType.CommunicationType terminalType, IFixture fixture)
    {
        var authenticationTypeResolver = new Mock<IResolveAuthenticationType>();
        authenticationTypeResolver.Setup<AuthenticationTypes>(a =>
                a.GetAuthenticationMethod(It.IsAny<TerminalCapabilities>(), It.IsAny<ApplicationInterchangeProfile>()))
            .Returns(AuthenticationTypes.CombinedDataAuthentication);

        fixture.Register(() => new TerminalActionAnalysisService(fixture.Create<IHandlePcdRequests>(), authenticationTypeResolver.Object,
            new TerminalType(TerminalType.Environment.Attended, terminalType, TerminalType.TerminalOperatorType.Merchant),
            fixture.Create<TerminalCapabilities>(), GetTerminalActionCodeDefault(), GetTerminalActionCodeDenial(),
            GetTerminalActionCodeOnline(), GetIssuerActionCodeDefault(), GetIssuerActionCodeDenial(), GetIssuerActionCodeOnline()));

        return fixture.Create<TerminalActionAnalysisService>();
    }

    public static TerminalActionAnalysisService Create(
        TerminalType.CommunicationType terminalType,
        TerminalCapabilities terminalCapabilities,
        IResolveAuthenticationType authenticationResolver,
        IFixture fixture)
    {
        fixture.Register(() => new TerminalActionAnalysisService(fixture.Create<IHandlePcdRequests>(), authenticationResolver,
            new TerminalType(TerminalType.Environment.Attended, terminalType, TerminalType.TerminalOperatorType.Merchant),
            terminalCapabilities, GetTerminalActionCodeDefault(), GetTerminalActionCodeDenial(), GetTerminalActionCodeOnline(),
            GetIssuerActionCodeDefault(), GetIssuerActionCodeDenial(), GetIssuerActionCodeOnline()));

        return fixture.Create<TerminalActionAnalysisService>();
    }

    public static ActionCodes GetRandomIssuerActionCodeOnline() =>
        IssuerActionCodesOnline.ElementAt(_Random.Next(0, IssuerActionCodesDefault.Count - 1));

    public static ActionCodes GetRandomIssuerActionCodeDefault() =>
        IssuerActionCodesDefault.ElementAt(_Random.Next(0, IssuerActionCodesDefault.Count - 1));

    public static ActionCodes GetRandomIssuerActionCodeDenial() =>
        IssuerActionCodesDenial.ElementAt(_Random.Next(0, IssuerActionCodesDenial.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeOnline() =>
        TerminalActionCodesOnline.ElementAt(_Random.Next(0, TerminalActionCodesDefault.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeDefault() =>
        TerminalActionCodesDefault.ElementAt(_Random.Next(0, TerminalActionCodesDefault.Count - 1));

    public static ActionCodes GetRandomTerminalActionCodeDenial() =>
        TerminalActionCodesDenial.ElementAt(_Random.Next(0, TerminalActionCodesDenial.Count - 1));

    #endregion

    #region Issuer Codes

    public static List<ActionCodes> IssuerActionCodesDenial = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ExpiredApplication),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ApplicationNotYetEffective)
    };

    public static List<ActionCodes> IssuerActionCodesOnline = new()
    {
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredPinPadPresentButPinWasNotEntered),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CardholderVerificationWasNotSuccessful),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
    };

    public static List<ActionCodes> IssuerActionCodesDefault = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.IssuerAuthenticationFailed),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ScriptProcessingFailedAfterFinalGenerateAc),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ScriptProcessingFailedBeforeFinalGenerateAc),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
    };

    #endregion

    #region Terminal Codes

    public static List<ActionCodes> TerminalActionCodesDenial = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.IccDataMissing),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ExpiredApplication),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.ApplicationNotYetEffective)
    };

    public static List<ActionCodes> TerminalActionCodesOnline = new()
    {
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.MerchantForcedTransactionOnline),
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.TransactionSelectedRandomlyForOnlineProcessing),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.TransactionExceedsFloorLimit),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.UpperConsecutiveOfflineLimitExceeded),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.UnrecognizedCvm),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
    };

    public static List<ActionCodes> TerminalActionCodesDefault = new()
    {
        new ActionCodes(
            (ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.PinEntryRequiredAndPinPadNotPresentOrNotWorking),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.OfflineDataAuthenticationWasNotPerformed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.CombinationDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.DynamicDataAuthenticationFailed),
        new ActionCodes((ulong) (TerminalVerificationResult) TerminalVerificationResultCodes.StaticDataAuthenticationFailed)
    };

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

    #region Terminal Action Codes

    #endregion
}