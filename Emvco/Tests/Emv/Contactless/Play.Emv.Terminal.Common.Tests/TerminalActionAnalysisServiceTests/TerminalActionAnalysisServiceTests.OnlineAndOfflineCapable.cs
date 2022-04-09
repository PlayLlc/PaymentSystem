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

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests
{
    #region Instance Members

    #region Transaction Cryptogram - Offline

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram
    /// </summary>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Fact]
    public void OnlineAndOfflineCapableTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults(0));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.TransactionCryptogram, rapdu.GetCryptogramType());
    }

    #endregion

    #endregion

    #region Authorization Request Cryptogram - Online

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeOnline), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeOnline), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void
        OnlineAndOfflineCapableTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
            ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, rapdu.GetCryptogramType());
    }

    #endregion

    #region Application Authentication Cryptogram - Deny

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
                                                                                 OnlineResponseOutcome.NotAvailable);

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode),
                                                                                 OnlineResponseOutcome.NotAvailable);

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void
        OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
            ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes),
                                             OnlineResponseOutcome.NotAvailable);

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="actionCode"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command = GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) actionCode));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    /// <summary>
    ///     OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
    /// </summary>
    /// <param name="terminalActionCode"></param>
    /// <param name="issuerActionCodes"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="Play.Emv.Ber.Exceptions.TerminalDataException"></exception>
    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDenial), 10,
                   MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
    {
        TerminalActionAnalysisService sut = new();
        TerminalActionAnalysisCommand command =
            GetTerminalActionAnalysisCommand(new TerminalVerificationResults((ulong) terminalActionCode | (ulong) issuerActionCodes));

        GenerateApplicationCryptogramRequest rapdu =
            sut.Process(command.GetTransactionSessionId(), GetKernelDatabaseForOnlineAndOfflineCapable());

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), rapdu.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, rapdu.GetCryptogramType());
    }

    #endregion
}