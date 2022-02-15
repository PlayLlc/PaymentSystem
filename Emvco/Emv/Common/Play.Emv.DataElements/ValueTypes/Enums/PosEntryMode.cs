using Play.Core;

namespace Play.Emv.DataElements;

public record PosEntryMode : EnumObject<byte>
{
    #region Static Metadata

    public static readonly PosEntryMode Unknown = new(0);
    public static readonly PosEntryMode Manual = new(1);
    public static readonly PosEntryMode Magstripe = new(2);
    public static readonly PosEntryMode Barcode = new(3);
    public static readonly PosEntryMode OpticalCharacterRecognition = new(4);
    public static readonly PosEntryMode IccWithCvvCapabilities = new(5);

    /// <summary>
    ///     Proximity Integrated Circuit Card (PICC). Contactless EMV
    /// </summary>
    public static readonly PosEntryMode Picc = new(7);

    /// <summary>
    ///     The Cardholder's credentials are stored on file for this entry mode. Either the merchant uses a token or has the
    ///     PAN saved from a previous transaction attempt
    /// </summary>
    public static readonly PosEntryMode CredentialsOnFile = new(10);

    public static readonly PosEntryMode MagstripeFallbackForIcc = new(80);

    /// <summary>
    ///     Track 2 is extracted using the Magstripe capability. This entry mode is capable of checking CVV
    /// </summary>
    public static readonly PosEntryMode TrackData = new(90);

    /// <summary>
    ///     Account number auto entry via contactless magnetic stripe. Examples of magnetic stripe contactless payment systems
    ///     include Google Wallet and Apple Pay. In these systems card data (replaced by a token due to security and PCI
    ///     compliance considerations) is injected into a mobile device.
    /// </summary>
    public static readonly PosEntryMode Token = new(91);

    public static readonly PosEntryMode IccWithoutCvv = new(95);

    #endregion

    #region Constructor

    public PosEntryMode(byte value) : base(value)
    { }

    #endregion
}