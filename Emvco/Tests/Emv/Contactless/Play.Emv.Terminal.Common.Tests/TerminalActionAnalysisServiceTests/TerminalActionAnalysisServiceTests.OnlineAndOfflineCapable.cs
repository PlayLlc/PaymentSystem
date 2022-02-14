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

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeOnline), 100,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void OfflineAndOnlineCapableTerminal_WithoutDenialResult_GeneratesAuthenticationRequestCryptogram(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut =
            TerminalActionAnalysisServiceFactory.Create(TerminalType.CommunicationType.OnlineAndOfflineCapable, _Fixture);

        TerminalVerificationResults results = new((ulong) actionCode);

        TerminalActionAnalysisCommand command = new(_Fixture.Freeze<TransactionSessionId>(), results,
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.TransactionCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    [Theory]
    [MemberData(nameof(TerminalActionAnalysisServiceFixture.GetRandomTerminalActionCodeDenial), 100,
        MemberType = typeof(TerminalActionAnalysisServiceFixture))]
    public void Temp(ActionCodes actionCode)
    {
        TerminalActionAnalysisService sut =
            TerminalActionAnalysisServiceFactory.Create(TerminalType.CommunicationType.OnlineAndOfflineCapable, _Fixture);

        TerminalVerificationResults results = new((ulong) actionCode);

        TerminalActionAnalysisCommand command = new(_Fixture.Freeze<TransactionSessionId>(), results,
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

        sut.Process(command);

        Assert.Equal(_Fixture.Freeze<TransactionSessionId>(), _TestSpy.GetTransactionSessionId());
        Assert.Equal(CryptogramTypes.ApplicationAuthenticationCryptogram, _TestSpy.GetCryptogramInformationData()!.GetCryptogramType());
    }

    #endregion
}