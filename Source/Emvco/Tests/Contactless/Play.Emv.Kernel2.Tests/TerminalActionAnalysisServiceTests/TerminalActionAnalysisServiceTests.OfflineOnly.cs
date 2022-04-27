//using System;

//using AutoFixture;

//using Play.Ber.Exceptions;
//using Play.Emv.Ber.Enums;
//using Play.Emv.Ber.Exceptions;
//using Play.Emv.Ber.ValueTypes;
//using Play.Emv.Identifiers;
//using Play.Emv.Kernel.Services;
//using Play.Testing.BaseTestClasses;

//using Xunit;

//namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

//public partial class TerminalActionAnalysisServiceTests : TestBase
//{
//    #region Transaction Cryptogram - Offline

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OfflineOnlyTerminal_WithDefaultTerminalVerificationResults_GeneratesTransactionCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();

//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OfflineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesTransactionCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();

//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OfflineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesTransactionCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();

//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));
//        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
//    }

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeOnline_GeneratesTransactionCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));
//        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
//    }

//    #endregion

//    #region Application Authentication Cryptogram - Deny

//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Fact]
//    public void OfflineOnlyTerminal_WithTerminalActionCodeDefault_GeneratesAuthenticationRequestCryptogram()
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
//    }

//    /// <summary>
//    ///     OfflineOnlyTerminal_WithIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram
//    /// </summary>
//    /// <param name="actionCode"></param>
//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Theory]
//    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
//    public void OfflineOnlyTerminal_WithIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
//    }

//    /// <summary>
//    ///     OfflineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
//    /// </summary>
//    /// <param name="actionCode"></param>
//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Theory]
//    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
//    public void OfflineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
//    }

//    /// <summary>
//    ///     OfflineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
//    /// </summary>
//    /// <param name="actionCode"></param>
//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Theory]
//    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
//    public void OfflineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));
//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
//    }

//    /// <summary>
//    ///     OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram
//    /// </summary>
//    /// <param name="terminalActionCode"></param>
//    /// <param name="issuerActionCodes"></param>
//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Theory]
//    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDefault), 10,
//        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
//    public void OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(
//        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
//    }

//    /// <summary>
//    ///     OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram
//    /// </summary>
//    /// <param name="terminalActionCode"></param>
//    /// <param name="issuerActionCodes"></param>
//    /// <exception cref="InvalidOperationException"></exception>
//    /// <exception cref="BerParsingException"></exception>
//    /// <exception cref="TerminalDataException"></exception>
//    [Theory]
//    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalAndIssuerActionCodeDenial), 10,
//        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
//    public void OfflineOnlyTerminal_WithTerminalAndIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(
//        ActionCodes terminalActionCode, ActionCodes issuerActionCodes)
//    {
//        TerminalActionAnalysisService sut = new();
//        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
//        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));

//        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
//            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
//    }

//    #endregion
//}

