using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests : TestBase
{
    #region Transaction Cryptogram - Offline

    /// <summary>
    ///     OfflineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OfflineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults(0));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); });
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesTransactionCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesTransactionCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());

        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); }, Build.Equals.Message(CryptogramTypes.TransactionCryptogram, actual));
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesTransactionCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeOnline), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesTransactionCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); });
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesTransactionCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeOnline), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesTransactionCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); });
    }

    #endregion

    #region Application Authentication Cryptogram - Deny

    /// <summary>
    ///     OfflineOnlyTerminal_WithTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDefault), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());

        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());

        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());

        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());

        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <summary>
    ///     OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDenial), 10,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));
        GetKernelDatabaseForOfflineOnly(_Fixture);

        CryptogramTypes actual = sut.Process(command.GetTransactionSessionId(), _Fixture.Create<KernelDatabase>());

        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    #endregion
}