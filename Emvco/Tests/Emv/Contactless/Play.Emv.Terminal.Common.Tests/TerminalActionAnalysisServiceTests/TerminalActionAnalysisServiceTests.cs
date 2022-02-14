using AutoFixture;
using AutoFixture.AutoMoq;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements;
using Play.Emv.Icc;
using Play.Emv.Sessions;
using Play.Emv.Terminal.Common.Services.TerminalActionAnalysis.Terminal;
using Play.Emv.Terminal.Contracts.Messages.Commands;

using Xunit;

namespace Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

[Trait("Type", "Unit")]
public partial class TerminalActionAnalysisServiceTests
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
}