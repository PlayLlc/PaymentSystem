using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Services;

using Xunit;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests
{
    #region Authorization Request Cryptogram - Online

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesAuthorizationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assertion(() => { Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual); });
    }

    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));

        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual);
    }

    #endregion

    #region Application Authentication Cryptogram - Deny

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <param name="actionCode"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
    {
        TerminalActionAnalysisService sut = new();

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram()
    {
        TerminalActionAnalysisService sut = new();

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram()
    {
        TerminalActionAnalysisService sut = new();

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram()
    {
        TerminalActionAnalysisService sut = new();

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="TerminalDataException"></exception>
    [Fact]
    public void OnlineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram()
    {
        TerminalActionAnalysisService sut = new();

        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineOnly(_Fixture));
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual);
    }

    #endregion
}