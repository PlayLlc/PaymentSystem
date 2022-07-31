using System;
using System.Linq.Expressions;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel.Services;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

public partial class TerminalActionAnalysisServiceTests : TestBase
{
    #region Transaction Cryptogram - Offline

    [Fact]
    public void OfflineOnlyTerminal_WithEmptyTerminalVerificationResults_GeneratesTransactionCryptogram()
    {
        TerminalActionAnalysisService sut = new();
        TransactionSessionId sessionId = _Fixture.Create<TransactionSessionId>();
        CryptogramTypes actual = sut.Process(sessionId, GetKernelDatabaseForOfflineOnly(_Fixture));
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
    }

    [Fact]
    public void OfflineOnlyTerminal_WithTerminalOnlineActionCodeSet_SkipsOnlineCheckAndGeneratesTransactionCryptogram()
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        TerminalVerificationResult terminalVerificationResult = new();
        terminalVerificationResult.SetOfflineDataAuthenticationWasNotPerformed();
        database.Update(new TerminalActionCodeOnline((ulong) terminalVerificationResult));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
    }

    [Fact]
    public void OfflineOnlyTerminal_WithIssuerOnlineActionCodeSet_SkipsOnlineCheckAndGeneratesTransactionCryptogram()
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        TerminalVerificationResult terminalVerificationResult = new();
        terminalVerificationResult.SetOfflineDataAuthenticationWasNotPerformed();
        database.Update(new IssuerActionCodeOnline((ulong) terminalVerificationResult));
        database.Update(new TerminalVerificationResults(terminalVerificationResult));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        //Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
    }

    [Fact]
    public void OfflineOnlyTerminal_WithDenialActionCodeSet_GeneratesApplicationAuthenticationCryptogram()
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        TerminalVerificationResult terminalVerificationResult = new();
        terminalVerificationResult.SetCombinationDataAuthenticationFailed();
        database.Update(new TerminalActionCodeDenial((ulong) terminalVerificationResult));
        database.Update(new TerminalVerificationResults(terminalVerificationResult));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDefault), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithIssuerActionCodeDefault_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        database.Update(new IssuerActionCodeDefault((ulong) actionCode));
        database.Update(new TerminalVerificationResults((ulong) actionCode));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeOnline), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithIssuerActionCodeOnline_GeneratesTransactionCryptogram(ActionCodes actionCode)
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        database.Update(new IssuerActionCodeOnline((ulong) actionCode));
        database.Update(new TerminalVerificationResults((ulong) actionCode));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomIssuerActionCodeDenial), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithIssuerActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        database.Update(new IssuerActionCodeDenial((ulong) actionCode));
        database.Update(new TerminalVerificationResults((ulong) actionCode));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDefault), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalActionCodeDefault_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        database.Update(new TerminalActionCodeDefault((ulong) actionCode));
        database.Update(new TerminalVerificationResults((ulong) actionCode));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalActionCodeOnline_GeneratesTransactionCryptogram(ActionCodes actionCode)
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        database.Update(new TerminalActionCodeOnline((ulong) actionCode));
        database.Update(new TerminalVerificationResults((ulong) actionCode));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.TransactionCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.TransactionCryptogram, (byte) actual));
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 10, MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithTerminalActionCodeDenial_GeneratesApplicationAuthenticationCryptogram(ActionCodes actionCode)
    {
        // Arrange
        TerminalActionAnalysisService sut = new();
        KernelDatabase database = GetKernelDatabaseForOfflineOnly(_Fixture);
        ClearActionCodes(database);
        database.Update(new TerminalActionCodeDenial((ulong) actionCode));
        database.Update(new TerminalVerificationResults((ulong) actionCode));

        // Act
        CryptogramTypes actual = sut.Process(_Fixture.Create<TransactionSessionId>(), database);

        // Assert
        Assertion(() => { Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, actual); },
            Build.Equals.Message((byte) CryptogramTypes.ApplicationAuthenticationCryptogram, (byte) actual));
    }

    #endregion
}