using System;
using System.Collections.Immutable;
using System.Linq;

using AutoFixture;

using Play.Ber.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing.Emv.Contactless;
using Play.Testing.Emv.Contactless.AutoFixture;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Services;

using Xunit;

using Play.Emv.Kernel2.Databases;
using Play.Emv.Security;
using Play.Encryption.Certificates;
using Play.Encryption.Ciphers.Hashing;
using Play.Globalization.Time;
using Play.Testing.BaseTestClasses;

namespace Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;

[Trait("Type", "Unit")]
public partial class TerminalActionAnalysisServiceTests : TestBase
{
    #region Instance Values

    private readonly IFixture _Fixture;
    private readonly KernelDatabase _Database;

    #endregion

    #region Constructor

    /// <summary>
    ///     ctor
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public TerminalActionAnalysisServiceTests()
    {
        try
        {
            _Fixture = new ContactlessFixture().Create();
            MessageIdentifiers? aaaa = _Fixture.Create<MessageIdentifiers>()!;
            CustomizeModuleObjects(_Fixture);

            _Fixture.Create<ScratchPad>();
            KernelDatabase a = _Fixture.Create<KernelDatabase>()!;
            _Database = a;
        }
        catch (Exception)
        {
            Console.WriteLine("ValueStreamMapping");
        }
    }

    #endregion

    #region Instance Members

    public sealed override void CustomizeModuleObjects(IFixture fixture)
    {
        fixture.Register<KnownObjects>(fixture.Create<Kernel2KnownObjects>);
    }

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