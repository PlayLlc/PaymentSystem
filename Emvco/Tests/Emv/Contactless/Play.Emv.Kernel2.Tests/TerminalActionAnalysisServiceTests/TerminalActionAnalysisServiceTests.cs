using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

using Play.Emv.Kernel2.Databases;
using Play.Testing.BaseTestClasses;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

[Trait("Type", "Unit")]
public partial class TerminalActionAnalysisServiceTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;

    #endregion

    #region Constructor

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public TerminalActionAnalysisServiceTests()
    {
        _Fixture = new ContactlessFixture().Create();
        CustomizeModuleObjects(_Fixture);
    }

    #endregion

    #region Instance Members

    public sealed override void CustomizeModuleObjects(IFixture fixture)
    {
        fixture.Register<KnownObjects>(fixture.Create<Kernel2KnownObjects>);
    }

    private void GetKernelDatabaseForOfflineOnly()
    {
        _Fixture.Register(() => new TerminalType(TerminalType.CommunicationType.OnlineOnly));
    }

    private void GetKernelDatabaseForOnlineOnly()
    {
        _Fixture.Register(() => new TerminalType(TerminalType.CommunicationType.OnlineOnly));
    }

    private void GetKernelDatabaseForOnlineAndOfflineCapable()
    {
        _Fixture.Register(() => new TerminalType(TerminalType.CommunicationType.OnlineAndOfflineCapable));
    }

    private TerminalActionAnalysisCommand GetTerminalActionAnalysisCommand(TerminalVerificationResults terminalVerificationResults) =>
        new(_Fixture.Freeze<TransactionSessionId>(), OutcomeParameterSet.Default, terminalVerificationResults, _Fixture.Create<ApplicationInterchangeProfile>(),
            _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());

    private TerminalActionAnalysisCommand GetTerminalActionAnalysisCommand(
        TerminalVerificationResults terminalVerificationResults, OnlineResponseOutcome onlineResponseOutcome)
    {
        OutcomeParameterSet.Builder outcomeBuilder = OutcomeParameterSet.GetBuilder();
        outcomeBuilder.Set(onlineResponseOutcome);

        return new TerminalActionAnalysisCommand(_Fixture.Freeze<TransactionSessionId>(), outcomeBuilder.Complete(), terminalVerificationResults,
            _Fixture.Create<ApplicationInterchangeProfile>(), _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());
    }

    #endregion
}