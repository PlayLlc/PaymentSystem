﻿using AutoFixture;
using AutoFixture.AutoMoq;

using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.Primitives.Card.Icc;
using Play.Emv.DataElements.Emv.Primitives.Outcome;
using Play.Emv.DataElements.Emv.Primitives.Terminal;
using Play.Emv.DataElements.Emv.ValueTypes;
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

    /// <summary>
    ///     ctor
    /// </summary>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="Play.Ber.Exceptions.BerException"></exception>
    public TerminalActionAnalysisServiceTests()
    {
        _Fixture = new Fixture();
        _Fixture.Customize(new AutoMoqCustomization());
        _TestSpy = GenerateApplicationCryptogramCommandTestSpy.Setup(_Fixture);
    }

    #endregion

    #region Instance Members

    private TerminalActionAnalysisService GetOfflineOnlyTerminalActionAnalysisService() =>
        TerminalActionAnalysisServiceFactory.Create(TerminalType.CommunicationType.OfflineOnly, _Fixture);

    private TerminalActionAnalysisService GetOnlineOnlyTerminalActionAnalysisService() =>
        TerminalActionAnalysisServiceFactory.Create(TerminalType.CommunicationType.OnlineOnly, _Fixture);

    private TerminalActionAnalysisService GetOnlineAndOfflineCapableTerminalActionAnalysisService() =>
        TerminalActionAnalysisServiceFactory.Create(TerminalType.CommunicationType.OnlineAndOfflineCapable, _Fixture);

    private TerminalActionAnalysisCommand GetTerminalActionAnalysisCommand(TerminalVerificationResults terminalVerificationResults) =>
        new(_Fixture.Freeze<TransactionSessionId>(), OutcomeParameterSet.Default, terminalVerificationResults,
            _Fixture.Create<ApplicationInterchangeProfile>(), _Fixture.Create<DataObjectListResult>(),
            _Fixture.Create<DataObjectListResult>());

    private TerminalActionAnalysisCommand GetTerminalActionAnalysisCommand(
        TerminalVerificationResults terminalVerificationResults,
        OnlineResponseOutcome onlineResponseOutcome)
    {
        OutcomeParameterSet.Builder outcomeBuilder = OutcomeParameterSet.GetBuilder();
        outcomeBuilder.Set(onlineResponseOutcome);

        return new TerminalActionAnalysisCommand(_Fixture.Freeze<TransactionSessionId>(), outcomeBuilder.Complete(),
            terminalVerificationResults, _Fixture.Create<ApplicationInterchangeProfile>(), _Fixture.Create<DataObjectListResult>(),
            _Fixture.Create<DataObjectListResult>());
    }

    #endregion
}