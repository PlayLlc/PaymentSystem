﻿using System;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.DataElements.Display;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing.Emv.Contactless.AutoFixture;

using Xunit;

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

        KernelDatabase database = _Fixture.Create<KernelDatabase>();
        database.Update(MessageHoldTime.MinimumValue);
    }

    #endregion

    #region Instance Members

    private static void GetKernelDatabaseForOfflineOnly(IFixture fixture)
    {
        KernelDatabase database = fixture.Create<KernelDatabase>();
        database.Update(new TerminalType(TerminalType.CommunicationType.OnlineOnly));
    }

    private void GetKernelDatabaseForOnlineOnly(IFixture fixture)
    {
        KernelDatabase database = fixture.Create<KernelDatabase>();
        database.Update(new TerminalType(TerminalType.CommunicationType.OnlineOnly));
    }

    private void GetKernelDatabaseForOnlineAndOfflineCapable(IFixture fixture)
    {
        KernelDatabase database = fixture.Create<KernelDatabase>();
        database.Update(new TerminalType(TerminalType.CommunicationType.OnlineAndOfflineCapable));
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