using AutoFixture;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests;

public static class TerminalRiskManagerFixture
{
    #region Instance Members

    public static IFixture RegisterGlobalizationCodes(this IFixture fixture)
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

    public static void RegisterTerminalRiskData(this IFixture fixture, ITlvReaderAndWriter kernelDatabase)
    {
        AmountAuthorizedNumeric authorizedAmount = fixture.Create<AmountAuthorizedNumeric>();
        kernelDatabase.Update(authorizedAmount);

        ApplicationPan primaryAccountNumber = fixture.Create<ApplicationPan>();
        kernelDatabase.Update(primaryAccountNumber);

        ApplicationCurrencyCode currencyCode = fixture.Create<ApplicationCurrencyCode>();
        kernelDatabase.Update(currencyCode);

        TerminalFloorLimit terminalFloorLimit = fixture.Create<TerminalFloorLimit>();
        kernelDatabase.Update(terminalFloorLimit);
    }

    #endregion
}