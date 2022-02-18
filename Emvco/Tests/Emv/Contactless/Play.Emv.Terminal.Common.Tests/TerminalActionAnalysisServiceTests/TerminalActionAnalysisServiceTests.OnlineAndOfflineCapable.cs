using AutoFixture;

using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;
using Play.Emv.Terminal.Contracts.Messages.Commands;

using Xunit;

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests
{
    #region Instance Members

    #region Transaction Cryptogram - Offline

    [Fact]
    public void OnlineAndOfflineCapableTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram()
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults(0));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.TransactionCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion

    #endregion

    #region Authorization Request Cryptogram - Online

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(
        ActionCodes terminalActionCode,
        ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void
        OnlineAndOfflineCapableTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
            ActionCodes terminalActionCode,
            ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
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
    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
            OnlineResponseOutcome.NotAvailable);

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
            OnlineResponseOutcome.NotAvailable);

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void
        OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
            ActionCodes terminalActionCode,
            ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(
            new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes), OnlineResponseOutcome.NotAvailable);

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode,
        ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = GetOnlineAndOfflineCapableTerminalActionAnalysisService();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion
}