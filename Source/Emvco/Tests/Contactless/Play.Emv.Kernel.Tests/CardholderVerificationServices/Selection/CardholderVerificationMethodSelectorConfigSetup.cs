using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Kernel.Tests.CardholderVerificationServices.Selection;

public static class CardholderVerificationMethodSelectorConfigSetup
{
    #region Instance Members

    public static IFixture RegisterGlobalizationProperties(this IFixture fixture)
    {
        NumericCountryCode numericCountryCode = new(840);
        fixture.Register(() => numericCountryCode);

        Alpha2CountryCode alpha2Code = new("US");
        fixture.Register(() => alpha2Code);

        Alpha2LanguageCode alpha2LanguageCode = new("en");
        fixture.Register(() => alpha2LanguageCode);

        NumericCurrencyCode numericCurrencyCode = new(840);
        fixture.Register(() => numericCurrencyCode);

        Alpha3CurrencyCode alpha3CurrencyCode = new("USD");
        fixture.Register(() => alpha3CurrencyCode);

        return fixture;
    }

    public static void RegisterDisabledConfigurationCvmDefaults(IFixture fixture, KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile = new(0b1000_0000_0000_0000);
        database.Update(applicationInterchangeProfile);

        KernelConfiguration kernelConfiguration = new(0b0000_0000);
        database.Update(kernelConfiguration);

        TerminalCapabilities terminalCapabilities = new(1234);
        database.Update(terminalCapabilities);

        ApplicationCurrencyCode applicationCurrencyCode = fixture.Create<ApplicationCurrencyCode>();
        database.Update(applicationCurrencyCode);

        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode = fixture.Create<TransactionReferenceCurrencyCode>();
        database.Update(transactionReferenceCurrencyCode);
    }

    public static void RegisterEnabledConfigurationCvmDefaults(IFixture fixture, KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile = new(0b1001_0010_0000_0000);
        database.Update(applicationInterchangeProfile);

        KernelConfiguration kernelConfiguration = new(0b0000_0000);
        database.Update(kernelConfiguration);

        TerminalCapabilities terminalCapabilities = new(1234);
        database.Update(terminalCapabilities);

        ApplicationCurrencyCode applicationCurrencyCode = fixture.Create<ApplicationCurrencyCode>();
        database.Update(applicationCurrencyCode);

        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode = fixture.Create<TransactionReferenceCurrencyCode>();
        database.Update(transactionReferenceCurrencyCode);
    }

    public static void RegisterTransactionAmountAndCvmThresholdValues(KernelDatabase database, ulong amountAuthorized, ulong cvmThreshold)
    {
        AmountAuthorizedNumeric amountAuthorizedNumeric = new(amountAuthorized);
        database.Update(amountAuthorizedNumeric);

        ReaderCvmRequiredLimit readerCvmRequiredLimit = new(cvmThreshold);
        database.Update(readerCvmRequiredLimit);
    }

    #endregion
}