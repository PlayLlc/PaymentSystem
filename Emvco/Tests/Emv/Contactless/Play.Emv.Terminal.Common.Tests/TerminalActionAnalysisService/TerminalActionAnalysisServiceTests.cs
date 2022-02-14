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