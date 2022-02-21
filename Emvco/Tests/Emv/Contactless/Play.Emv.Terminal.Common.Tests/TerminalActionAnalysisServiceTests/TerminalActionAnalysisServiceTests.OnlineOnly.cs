using AutoFixture;

using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Emv.Icc;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;
using Play.Emv.Terminal.Contracts.Messages.Commands;

using Xunit;

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests
{
    #region Authorization Request Cryptogram - Online

    [Fact]
    public void OnlineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesAuthorizationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = GetOfflineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults(0));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(
        ActionCodes terminalActionCode,
        ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode,
        ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion

    #region Application Authentication Cryptogram - Deny

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
            OnlineResponseOutcome.NotAvailable);

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
            OnlineResponseOutcome.NotAvailable);

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode,
        ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(
            new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes), OnlineResponseOutcome.NotAvailable);

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode,
        ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineOnlyTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion
}