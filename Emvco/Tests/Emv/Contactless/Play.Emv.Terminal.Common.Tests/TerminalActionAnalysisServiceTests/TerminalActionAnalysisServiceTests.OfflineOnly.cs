using AutoFixture;

using Play.Emv.Ber.DataObjects;
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

    private TerminalActionAnalysisService GetOfflineOnlyTerminalActionAnalysisService() =>
        TerminalActionAnalysisServiceFactory.Create(TerminalType.CommunicationType.OfflineOnly, _Fixture);

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 100,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithoutDenialResult_GeneratesTransactionCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOfflineOnlyTerminalActionAnalysisService();

        TerminalVerificationResults results = new((ulong) actionCode);

        TerminalActionAnalysisCommand command = new(_Fixture.Freeze<TransactionSessionId>(), results,
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.TransactionCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDefault), 100,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineOnlyTerminal_WithoutDenialResult_GeneratesTransactionCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOfflineOnlyTerminalActionAnalysisService();

        TerminalVerificationResults results = new((ulong) actionCode);

        TerminalActionAnalysisCommand command = new(_Fixture.Freeze<TransactionSessionId>(), results,
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.TransactionCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 100,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OnlineOnlyTerminal_WithoutDenialResult_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut = GetOfflineOnlyTerminalActionAnalysisService();
        TerminalVerificationResults results = new((ulong) actionCode);

        TerminalActionAnalysisCommand command = new(_Fixture.Freeze<TransactionSessionId>(), results,
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.TransactionCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion

    #region Authorization Result Cryptogram - Online

    #endregion

    #region Transaction Cryptogram - Offline

    #endregion

    #region Application Authentication Cryptogram - Deny

    #endregion
}