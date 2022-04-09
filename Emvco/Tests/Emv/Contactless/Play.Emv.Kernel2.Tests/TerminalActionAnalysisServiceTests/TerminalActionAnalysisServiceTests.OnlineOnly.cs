﻿using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Services;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.Messages.Commands;

using Xunit;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests
{
    #region Authorization Request Cryptogram - Online

    /// <summary>
    ///     OnlineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesAuthorizationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults(0));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineOnly());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeOnline), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineOnly());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeOnline), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineOnly());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineOnly());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineOnly());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    #endregion

    #region Application Authentication Cryptogram - Deny

    /// <summary>
    ///     OnlineOnlyTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
                                                                                 OnlineResponseOutcome.NotAvailable);

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
                                                                                 OnlineResponseOutcome.NotAvailable);

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes),
                                             OnlineResponseOutcome.NotAvailable);

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDenial), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        GenerateApplicationCryptogramRequest rapdu = sut.Process(command.GetTransactionSessionId(), _Database);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    #endregion
}