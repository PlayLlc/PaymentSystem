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
        CustomizeModuleObjects(_Fixture);
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

    #region Customize Module

    public sealed override void CustomizeModuleObjects(IFixture fixture)
    {
        CreateDatabase(fixture);
        SetupDatabase(fixture);
    }

    #region Database Customization

    private static void CreateDatabase(IFixture fixture)
    {
        fixture.Freeze<TransactionSessionId>();
        fixture.Freeze<KernelSessionId>();

        fixture.Register<KnownObjects>(fixture.Create<Kernel2KnownObjects>);
        KernelDatabase database = fixture.Create<KernelDatabase>();
        database.Activate(fixture.Create<KernelSessionId>());
        fixture.Register(() => database);
    }

    private static void SetupDatabase(IFixture fixture)
    {
        KernelDatabase database = fixture.Create<KernelDatabase>();
        TagsToRead tagsToRead = new();
        TerminalTransactionQualifiers ttq = fixture.Create<TerminalTransactionQualifiers>();
        SelectApplicationDefinitionFileInfoResponse rapdu = CreateSelectApplicationDefinitionFileInfoResponse(fixture);

        database.Update(ttq);
        database.Update(tagsToRead);
        database.Update(rapdu.AsPrimitiveValues());
        database.Update(CreateCombinationCompositeKey(fixture).AsPrimitiveValues());
        database.Update(Outcome.Default.AsPrimitiveValues());
        database.Update(GetTransaction(fixture).AsPrimitiveValues());
    }

    private static CombinationCompositeKey CreateCombinationCompositeKey(IFixture fixture)
    {
        DedicatedFileName dedicatedFileName = fixture.Create<DedicatedFileName>();
        ShortKernelIdTypes kernelType = ShortKernelIdTypes.Kernel2;
        Transaction transaction = GetTransaction(fixture);

        return new CombinationCompositeKey(dedicatedFileName, kernelType, transaction.GetTransactionType());
    }

    private static SelectApplicationDefinitionFileInfoResponse CreateSelectApplicationDefinitionFileInfoResponse(IFixture fixture)
    {
        CorrelationId correlationId = fixture.Create<CorrelationId>();

        return new SelectApplicationDefinitionFileInfoResponse(correlationId, fixture.Create<TransactionSessionId>(),
            CreateGetFileControlInformationRApduSignal(fixture));
    }

    private static GetFileControlInformationRApduSignal CreateGetFileControlInformationRApduSignal(IFixture fixture)
    {
        FileControlInformationAdf fci = fixture.Create<FileControlInformationAdf>();
        Span<byte> rapdu = new byte[fci.GetTagLengthValueByteCount() + StatusWords.GetByteCount()];
        StatusWords._9000.Encode().CopyTo(rapdu);
        fci.EncodeTagLengthValue().CopyTo(rapdu[2..]);

        return new GetFileControlInformationRApduSignal(rapdu.ToArray());
    }

    private static Transaction GetTransaction(IFixture fixture)
    {
        TransactionDate transactionDate = fixture.Create<TransactionDate>();
        TransactionTime transactionTime = fixture.Create<TransactionTime>();
        TransactionSessionId transactionSessionId = fixture.Create<TransactionSessionId>();
        TransactionType? transactionType = fixture.Create<TransactionType>();
        LanguagePreference languagePreference = fixture.Create<LanguagePreference>();
        AmountAuthorizedNumeric amountAuthorized = fixture.Create<AmountAuthorizedNumeric>();
        AmountOtherNumeric amountOther = fixture.Create<AmountOtherNumeric>();
        AccountType accountType = fixture.Create<AccountType>();
        TerminalCountryCode terminalCountryCode = fixture.Create<TerminalCountryCode>();

        return new Transaction(transactionSessionId, accountType, amountAuthorized, amountOther, transactionType, languagePreference, terminalCountryCode,
            transactionDate, transactionTime);
    }

    #endregion

    #endregion
}