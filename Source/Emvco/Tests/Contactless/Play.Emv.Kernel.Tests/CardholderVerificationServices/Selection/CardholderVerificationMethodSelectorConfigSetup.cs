using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Kernel.Tests.CardholderVerificationServices.Selection;

public static class CardholderVerificationMethodSelectorConfigSetup
{
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

    public static void RegisterDisabledConfigurationCVMDefaults(IFixture fixture, KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile = new ApplicationInterchangeProfile(0b1000_0000_0000_0000);
        database.Update(applicationInterchangeProfile);

        KernelConfiguration kernelConfiguration = new KernelConfiguration(0b0000_0000);
        database.Update(kernelConfiguration);

        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(1234);
        database.Update(terminalCapabilities);

        ApplicationCurrencyCode applicationCurrencyCode = fixture.Create<ApplicationCurrencyCode>();
        database.Update(applicationCurrencyCode);

        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode = fixture.Create<TransactionReferenceCurrencyCode>();
        database.Update(transactionReferenceCurrencyCode);
    }

    public static void RegisterEnabledConfigurationCVMDefaults(IFixture fixture, KernelDatabase database)
    {
        ApplicationInterchangeProfile applicationInterchangeProfile = new ApplicationInterchangeProfile(0b1001_0010_0000_0000);
        database.Update(applicationInterchangeProfile);

        KernelConfiguration kernelConfiguration = new KernelConfiguration(0b0000_0000);
        database.Update(kernelConfiguration);

        TerminalCapabilities terminalCapabilities = new TerminalCapabilities(1234);
        database.Update(terminalCapabilities);

        ApplicationCurrencyCode applicationCurrencyCode = fixture.Create<ApplicationCurrencyCode>();
        database.Update(applicationCurrencyCode);

        TransactionReferenceCurrencyCode transactionReferenceCurrencyCode = fixture.Create<TransactionReferenceCurrencyCode>();
        database.Update(transactionReferenceCurrencyCode);
    }

    public static void RegisterTransactionAmountAndCVMTresholdValues(KernelDatabase database, ulong amountAuthorized, ulong cvmThreshold)
    {
        AmountAuthorizedNumeric amountAuthorizedNumeric = new AmountAuthorizedNumeric(amountAuthorized);
        database.Update(amountAuthorizedNumeric);

        ReaderCvmRequiredLimit readerCvmRequiredLimit = new ReaderCvmRequiredLimit(cvmThreshold);
        database.Update(readerCvmRequiredLimit);
    }
}
