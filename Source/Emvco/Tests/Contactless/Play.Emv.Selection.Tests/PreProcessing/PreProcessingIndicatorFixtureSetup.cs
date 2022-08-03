﻿using AutoFixture;

using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Emv.Identifiers;
using Play.Globalization;
using Play.Globalization.Country;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Selection.Tests.PreProcessing;

public static class PreProcessingIndicatorFixtureSetup
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

        ShortKernelIdTypes kernelId = ShortKernelIdTypes.Kernel1;
        fixture.Register(() => kernelId);

        return fixture;
    }

    public static void RegisterTerminalTransactionQualifiers(this IFixture fixture, uint? ttqValue = null)
    {
        TerminalTransactionQualifiers ttq = ttqValue.HasValue
            ? new TerminalTransactionQualifiers(ttqValue.Value)
            : fixture.Create<TerminalTransactionQualifiers>();

        fixture.Register(() => ttq);
    }

    public static void RegisterReaderContactlessTransactionLimit(this IFixture fixture, ulong amount)
    {
        ReaderContactlessTransactionLimitWhenCvmIsOnDevice readerContactlessTransactionLimit = new(amount);

        fixture.Register<ReaderContactlessTransactionLimit>(() => readerContactlessTransactionLimit);
    }

    public static void RegisterReaderCvmRequiredLimit(this IFixture fixture, ulong amount)
    {
        ReaderCvmRequiredLimit readerCvmRequiredLimit = new(amount);

        fixture.Register(() => readerCvmRequiredLimit);
    }

    public static void RegisterTerminalFloorLimit(this IFixture fixture, uint tFloorLimit)
    {
        TerminalFloorLimit terminalFloorLimit = new(tFloorLimit);

        fixture.Register(() => terminalFloorLimit);
    }

    public static void RegisterTerminalCategoriesSupportedList(this IFixture fixture)
    {
        TerminalCategoriesSupportedList terminalCategoriesSupportedList = fixture.Create<TerminalCategoriesSupportedList>();

        fixture.Register(() => terminalCategoriesSupportedList);
    }

    public static void RegisterCombinationCompositeKey(this IFixture fixture)
    {
        CombinationCompositeKey combinationCompositeKey = fixture.Create<CombinationCompositeKey>();
        fixture.Freeze<CombinationCompositeKey>();
    }

    public static void RegisterAmmountAuthorizedNumeric(this IFixture fixture, ulong amount)
    {
        AmountAuthorizedNumeric amountAuthorizedNumeric = new AmountAuthorizedNumeric(amount);
        fixture.Register(() => amountAuthorizedNumeric);
    }

    public static void RegisterCultureProfile(this IFixture fixture)
    {
        CultureProfile cultureProfile = fixture.Create<CultureProfile>();
        fixture.Freeze<CultureProfile>();
    }

    #endregion
}
