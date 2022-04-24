﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using Play.Emv;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Templates;
using Play.Emv.Icc;
using Play.Emv.Identifiers;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Outcomes;
using Play.Emv.Pcd.Contracts;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Icc.Messaging.Apdu;
using Play.Messaging;

namespace Play.Testing.Emv.Contactless.AutoFixture
{
    public partial class ContactlessFixture
    {
        #region Database Customization

        public static void RegisterDefaultDatabase(IFixture fixture)
        {
            CreateDatabase(fixture);
            SetupDatabase(fixture);
        }

        private static void CreateDatabase(IFixture fixture)
        {
            fixture.Freeze<TransactionSessionId>();
            fixture.Freeze<KernelSessionId>();

            fixture.Register<KnownObjects>(fixture.Create<Kernel2KnownObjects>);
            KernelDatabase database = fixture.Create<KernelDatabase>();
            database.Activate(fixture.Create<KernelSessionId>());
            fixture.Freeze<KernelDatabase>();
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
            TransactionType transactionType = fixture.Create<TransactionType>();
            LanguagePreference languagePreference = fixture.Create<LanguagePreference>();
            AmountAuthorizedNumeric amountAuthorized = fixture.Create<AmountAuthorizedNumeric>();
            AmountOtherNumeric amountOther = fixture.Create<AmountOtherNumeric>();
            AccountType accountType = fixture.Create<AccountType>();
            TerminalCountryCode terminalCountryCode = fixture.Create<TerminalCountryCode>();

            TransactionCurrencyExponent transactionCurrencyExponent = fixture.Create<TransactionCurrencyExponent>();
            TransactionCurrencyCode transactionCurrencyCode = fixture.Create<TransactionCurrencyCode>();

            return new Transaction(transactionSessionId, accountType, amountAuthorized, amountOther, transactionType, languagePreference, terminalCountryCode,
                transactionDate, transactionTime, transactionCurrencyExponent, transactionCurrencyCode);
        }

        #endregion
    }
}