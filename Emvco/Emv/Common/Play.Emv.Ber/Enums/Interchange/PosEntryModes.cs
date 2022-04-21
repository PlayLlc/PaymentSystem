using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.Enums.Interchange;

/// <summary>
///     Card brands such as Mastercard and Visa will have proprietary POS Entry Mode values they use internally
/// </summary>
public record PosEntryModes : EnumObject<byte>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<ushort, PosEntryModes> _ValueObjectMap = new Dictionary<ushort, PosEntryModes>
    {
        {Unknown, Unknown},
        {ManualEntry, ManualEntry},
        {Magstripe, Magstripe},
        {TrackData, TrackData},
        {ManualEntryFallback, ManualEntryFallback},
        {Contact, Contact},
        {ContactWithoutCvvFallback, ContactWithoutCvvFallback},
        {MagstripeFallback, MagstripeFallback},
        {Token, Token},
        {OnFile, OnFile},
        {Ecommerce, Ecommerce},
        {QrCode, QrCode},
        {Barcode, Barcode},
        {Contactless, Contactless}
    }.ToImmutableSortedDictionary();

    private static readonly PosEntryModes _Unknown = new(0);
    private static readonly PosEntryModes _ManualEntry = new(1);
    private static readonly PosEntryModes _Magstripe = new(2);
    private static readonly PosEntryModes _Barcode = new(3);
    private static readonly PosEntryModes _OpticalCharacterRecognition = new(4);
    private static readonly PosEntryModes _Icc = new(5);
    private static readonly PosEntryModes _Picc = new(7);
    private static readonly PosEntryModes _CredentialsOnFile = new(10);
    private static readonly PosEntryModes _ManualEntryFallbackForIccOrMagstripe = new(79);
    private static readonly PosEntryModes _MagstripeFallbackForIcc = new(80);
    private static readonly PosEntryModes _ManualEntryForEcommerce = new(81);
    private static readonly PosEntryModes _TrackData = new(90);
    private static readonly PosEntryModes _Token = new(91);
    private static readonly PosEntryModes _IccWithoutCvv = new(95);
    public static readonly PosEntryModes Unknown = _Unknown;
    public static readonly PosEntryModes ManualEntry = _ManualEntry;

    /// <summary>
    ///     The terminal automates the extraction of Track 2 data after reading the Magstripe
    /// </summary>
    public static readonly PosEntryModes TrackData = _TrackData;

    public static readonly PosEntryModes Magstripe = _Magstripe;

    /// <summary>
    ///     Final fallback mode used when the other EMV Modes are unavailable
    /// </summary>
    public static readonly PosEntryModes ManualEntryFallback = _ManualEntryFallbackForIccOrMagstripe;

    public static readonly PosEntryModes Contact = _Icc;

    /// <summary>
    ///     Contactless EMV mode. The terminal interacts with the Proximity Integrated Circuit Card (PICC) through a Proximity
    ///     Coupling Device
    /// </summary>
    public static readonly PosEntryModes Contactless = _Picc;

    /// <summary>
    ///     Fallback mode used when the EMV data might be considered unreliable, such as not including the CVV value in the
    ///     Track2 equivalent data
    /// </summary>
    public static readonly PosEntryModes ContactWithoutCvvFallback = _IccWithoutCvv;

    /// <summary>
    ///     Fallback mode used when either a Contact or Contactless EMV Mode has been attempted
    /// </summary>
    public static readonly PosEntryModes MagstripeFallback = _MagstripeFallbackForIcc;

    /// <summary>
    ///     Account number auto entry via contactless magnetic stripe. Examples of magnetic stripe contactless payment systems
    ///     include Google Wallet and Apple Pay. In these systems card data (replaced by a token due to security and PCI
    ///     compliance considerations) is injected into a mobile device.
    /// </summary>
    public static readonly PosEntryModes Token = _Token;

    /// <summary>
    ///     The Cardholder's credentials are stored on file for this entry mode. Either the merchant uses a token or has the
    ///     PAN saved from a previous transaction attempt
    /// </summary>
    public static readonly PosEntryModes OnFile = _CredentialsOnFile;

    public static readonly PosEntryModes Ecommerce = _ManualEntryForEcommerce;
    public static readonly PosEntryModes QrCode = _OpticalCharacterRecognition;
    public static readonly PosEntryModes Barcode = _Barcode;

    #endregion

    #region Constructor

    public PosEntryModes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PosEntryModes[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out PosEntryModes? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static PosEntryModes[] GetAll() => _ValueObjectMap.Values.ToArray();

    #endregion
}