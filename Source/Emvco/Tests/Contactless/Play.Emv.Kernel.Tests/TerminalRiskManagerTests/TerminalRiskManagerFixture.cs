using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Databases;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public static class TerminalRiskManagerFixture
{
    public static IFixture RegisterGlobalizationCodes(this IFixture fixture)
    {
        NumericCountryCode numericCountryCode = new NumericCountryCode(840);
        fixture.Register(() =>
        numericCountryCode);

        Alpha2CountryCode alpha2Code = new Alpha2CountryCode("US");
        fixture.Register(() =>
        alpha2Code);

        Alpha2LanguageCode alpha2LanguageCode = new Alpha2LanguageCode("en");
        fixture.Register(() =>
        alpha2LanguageCode);

        NumericCurrencyCode numericCurrencyCode = new NumericCurrencyCode(840);
        fixture.Register(() =>
        numericCurrencyCode);

        Alpha3CurrencyCode alpha3CurrencyCode = new Alpha3CurrencyCode("USD");
        fixture.Register(() =>
        alpha3CurrencyCode);

        return fixture;
    }

    public static void RegisterTerminalRiskData(this IFixture fixture, ITlvReaderAndWriter kernelDatabase)
    {
        AmountAuthorizedNumeric authorizedAmmount = fixture.Create<AmountAuthorizedNumeric>();
        kernelDatabase.Update(authorizedAmmount);

        ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
        kernelDatabase.Update(primaryAccountNumber);

        ApplicationCurrencyCode currencyCode = fixture.Create<ApplicationCurrencyCode>();
        kernelDatabase.Update(currencyCode);

        TerminalFloorLimit terminalFloorLimit = fixture.Create<TerminalFloorLimit>();
        kernelDatabase.Update(terminalFloorLimit);
    }
}
