//using System;

//using AutoFixture;

//using Play.Ber.Exceptions;
//using Play.Emv.Ber.Enums;
//using Play.Emv.Ber.Exceptions;
//using Play.Emv.Identifiers;
//using Play.Emv.Kernel.Services;

//using Xunit;

//namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

//public partial class TerminalActionAnalysisServiceTests
//{
//    #region Instance Members

//    #region Transaction Cryptogram - Offline

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram()
//    {
//        TerminalVerificationResultCodes test = TerminalVerificationResultCodes.StaticDataAuthenticationFailed;

//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));
//        Assert.Equal(CryptogramTypes.TransactionCryptogram, actual);
//    }

//    #endregion

//    #endregion

//    #region Authorization Request Cryptogram - Online

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTerminalActionCodeOnline_GeneratesAuthorizationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesAuthorizationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithoutTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));
//        Assertion(() => { Assert.Equal(CryptogramTypes.AuthorizationRequestCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithoutTimeoutTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    #endregion

//    #region Application Authentication Cryptogram - Deny

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTimeoutAndTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OnlineAndOfflineCapableTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOnlineAndOfflineCapable(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); });
//    }

//    #endregion
//}

