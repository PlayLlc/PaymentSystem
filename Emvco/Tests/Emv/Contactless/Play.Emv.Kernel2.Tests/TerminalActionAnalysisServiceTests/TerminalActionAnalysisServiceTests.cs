using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Templates;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

using Play.Emv.Kernel2.Databases;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;
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
        ContactlessFixture.RegisterDefaultDatabase(_Fixture);
    }

    #endregion

    #region Instance Members

    private static void GetKernelDatabaseForOfflineOnly(IFixture fixture)
    {
        fixture.Register(() => new TerminalType(TerminalType.CommunicationType.OnlineOnly));
    }

    private void GetKernelDatabaseForOnlineOnly(IFixture fixture)
    {
        fixture.Register(() => new TerminalType(TerminalType.CommunicationType.OnlineOnly));
    }

    private void GetKernelDatabaseForOnlineAndOfflineCapable(IFixture fixture)
    {
        fixture.Register(() => new TerminalType(TerminalType.CommunicationType.OnlineAndOfflineCapable));
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