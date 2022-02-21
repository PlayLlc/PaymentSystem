using Play.Emv.DataElements;
using Play.Emv.DataElements.Emv;
using Play.Globalization;
using Play.Globalization.Currency;
using Play.Globalization.Language;

namespace Play.Emv.Pos;

public abstract class PosConfiguration
{
    #region Instance Values

    /// <summary>
    ///     If the value of Autorun is “No”, then the transaction start is initiated by the merchant, typically by entering the
    ///     amount. If the value of Autorun is “Yes”, then the transaction start is when a card enters the polling field and
    ///     responds, indicating its presence.
    /// </summary>
    protected readonly bool _Autorun;

    /// <summary>
    ///     Identifies the cultures supported by the POS
    /// </summary>
    protected readonly CultureProfile _LocalCulture;

    // TODO: Putting this here for now but I don't think it belongs here
    //public readonly PoiInformation PoiInformation;
    protected readonly TransactionType[] _SupportedTransactions;

    #endregion

    #region Constructor

    protected PosConfiguration(CultureProfile localCulture, bool autorun = false)
    {
        _LocalCulture = localCulture;
        _Autorun = autorun;
    }

    #endregion

    #region Instance Members

    public CultureProfile GetPosLocalCulture() => _LocalCulture;
    public NumericCurrencyCode GetPosLocalCurrency() => _LocalCulture.GetNumericCurrencyCode();
    public Alpha2LanguageCode GetPosLocalLanguage() => _LocalCulture.GetAlpha2LanguageCode();

    #endregion

    // protected readonly OperatingModes[] _SupportedOperatingModes; - EMVContact, EMVContactless, Mag, etc

    // Dictionary<CombinationCompositeKey, {KernelConfiguration, EntryPointConfiguration}>;
}