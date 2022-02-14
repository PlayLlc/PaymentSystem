using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoMoq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Icc.GenerateApplicationCryptogram;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Messaging;
using Play.Randoms;

using Xunit;

namespace Play.Emv.Terminal.Common.Tests;

internal class TerminalActionAnalysisServiceFixture
{
    #region Static Metadata

    private static readonly Random _Random = new();

    #endregion

    #region Instance Members

    public static IEnumerable<object[]> GetRandomTerminalActionCodeDenial(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeDenial()};
        }
    }

    public static IEnumerable<object[]> GetRandomTerminalActionCodeDefault(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeDefault()};
    }

    public static IEnumerable<object[]> GetRandomTerminalActionCodeOnline(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomTerminalActionCodeOnline()};
    }

    public static IEnumerable<object[]> GetRandomIssuerActionCodeOnline(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeOnline()};
    }

    public static IEnumerable<object[]> GetRandomIssuerActionCodeDefault(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeDefault()};
    }

    public static IEnumerable<object[]> GetRandomIssuerActionCodeDenial(int count)
    {
        for (int i = 0; i < count; i++)
            yield return new object[] {TerminalActionAnalysisServiceFactory.GetRandomIssuerActionCodeDenial()};
    }

    #endregion
}

[Trait("Type", "Unit")]
public class TerminalActionAnalysisServiceTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly GenerateApplicationCryptogramCommandTestSpy _TestSpy;

    #endregion

    #region Constructor

    public TerminalActionAnalysisServiceTests()
    {
        _Fixture = new Fixture();
        _Fixture.Customize(new AutoMoqCustomization());
        _TestSpy = GenerateApplicationCryptogramCommandTestSpy.Setup(_Fixture);
    }

    #endregion

    #region Instance Members

    #region Terminal Online Tests

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 100,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void Temp(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = TerminalActionAnalysisServiceFactory.Create(
            new TerminalType(TerminalType.Environment.Attended, TerminalType.CommunicationType.OfflineOnly,
                TerminalType.TerminalOperatorType.FinancialInstitution), _Fixture);

        TerminalVerificationResults results = new((ulong) actionCode);

        TerminalActionAnalysisCommand command = new(_Fixture.Freeze<TransactionSessionId>(), results,
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion

    [Fact]
    public void OfflineOnlyTerminal_DenialIsNotSet_GeneratesTransactionCryptogram()
    {
        TerminalActionAnalysisService sut = TerminalActionAnalysisServiceFactory.Create(
            new TerminalType(TerminalType.Environment.Attended, TerminalType.CommunicationType.OfflineOnly,
                TerminalType.TerminalOperatorType.FinancialInstitution), _Fixture);

        TerminalVerificationResults a = new TerminalVerificationResults(TerminalVerificationResult.Create().var a =
            new TerminalActionAnalysisCommand(_Fixture.Create<TransactionSessionId>(), _Fixture.Create<DataObjectListResult>(),
                _Fixture.Create<DataObjectListResult>());

        _TerminalActionAnalysisService.Process();

        _PcdEndpoint.Request(_Fixture.Create<ActivatePcdRequest>());
        _PcdEndpoint.
    }

    [Fact]
    public void OnlineOnlyTerminal_DenialIsNotSet_GeneratesAuthorizationRequestCryptogram()
    {
        Random a = new();

        ActionCodes bb =
            TerminalActionAnalysisServiceFactory.IssuerDefaultActionCodes.ElementAt(a.Next(0,
                TerminalActionAnalysisServiceFactory.IssuerDefaultActionCodes.Count - 1));

        TerminalActionAnalysisServiceFactory.IssuerDefaultActionCodes.Count
    }

    [Fact]
    public void OnlineOnlyTerminal_DenialIsNotSet_GeneratesApplicationAuthenticationCryptogram()
    { }

    #endregion

    #region Terminal Denial Tests

    #endregion

    #region Terminal Default Tests

    #endregion

    #region Issuer Online Tests

    #endregion

    #region Issuer Denial Tests

    #endregion

    #region Issuer Default Tests

    #endregion

    #region Issuer & Terminal Online Tests

    #endregion

    #region Issuer & Terminal Denial Tests

    #endregion

    #region Issuer & Terminal Default Tests

    #endregion
}