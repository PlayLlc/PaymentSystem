using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing.Infrastructure.AutoFixture.FixtureFactories;

using Xunit;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

[Trait("Type", "Unit")]
public partial class TerminalActionAnalysisServiceTests
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly KernelDatabase _Database;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <exception cref="System.InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public TerminalActionAnalysisServiceTests()
    {
        _Fixture = DefaultFixtureFactory.Create();
        _Database = _Fixture.Create<KernelDatabase>();

        //RegisteredApplicationProviderIndicator
    }

    #endregion

    #region Instance Members

    private KernelDatabase GetKernelDatabaseForOfflineOnly()
    {
        KernelDatabaseFactory.KernelDatabaseBuilder? builder = KernelDatabaseFactory.GetBuilder();
        builder.SetOfflineOnlyTerminal();

        return builder.Complete();
    }

    private KernelDatabase GetKernelDatabaseForOnlineOnly()
    {
        KernelDatabaseFactory.KernelDatabaseBuilder? builder = KernelDatabaseFactory.GetBuilder();
        builder.SetOnlineOnlyTerminal();

        return builder.Complete();
    }

    private KernelDatabase GetKernelDatabaseForOnlineAndOfflineCapable()
    {
        KernelDatabaseFactory.KernelDatabaseBuilder? builder = KernelDatabaseFactory.GetBuilder();
        builder.SetOnlineAndOfflineCapableTerminal();

        return builder.Complete();
    }

    private TerminalActionAnalysisCommand GetTerminalActionAnalysisCommand(TerminalVerificationResults terminalVerificationResults) =>
        new(_Fixture.Freeze<TransactionSessionId>(), OutcomeParameterSet.Default, terminalVerificationResults,
            _Fixture.Create<ApplicationInterchangeProfile>(), _Fixture.Create<DataObjectListResult>(),
            _Fixture.Create<DataObjectListResult>());

    private TerminalActionAnalysisCommand GetTerminalActionAnalysisCommand(
        TerminalVerificationResults terminalVerificationResults, OnlineResponseOutcome onlineResponseOutcome)
    {
        OutcomeParameterSet.Builder outcomeBuilder = OutcomeParameterSet.GetBuilder();
        outcomeBuilder.Set(onlineResponseOutcome);

        return new TerminalActionAnalysisCommand(_Fixture.Freeze<TransactionSessionId>(), outcomeBuilder.Complete(),
                                                 terminalVerificationResults, _Fixture.Create<ApplicationInterchangeProfile>(),
                                                 _Fixture.Create<DataObjectListResult>(), _Fixture.Create<DataObjectListResult>());
    }

    #endregion
}