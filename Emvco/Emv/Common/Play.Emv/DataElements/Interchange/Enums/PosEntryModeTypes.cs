using Play.Core;

namespace Play.Emv.DataElements;

/// <summary>
///     Card brands such as Mastercard and Visa will have proprietary POS Entry Mode values they use internally
/// </summary>
public record PosEntryModeTypes : EnumObject<byte>
{
    #region Static Metadata

    public static readonly PosEntryModeTypes Unknown = new PosEntryModeTypes(0);
    public static readonly PosEntryModeTypes ManualEntry = new PosEntryModeTypes(1);
    private static readonly PosEntryModeTypes _Magstripe = new PosEntryModeTypes(2);
    private static readonly PosEntryModeTypes _Barcode = new PosEntryModeTypes(3);
    private static readonly PosEntryModeTypes _OpticalCharacterRecognition = new PosEntryModeTypes(4);
    private static readonly PosEntryModeTypes _Icc = new PosEntryModeTypes(5);
    private static readonly PosEntryModeTypes _Picc = new PosEntryModeTypes(7);
    private static readonly PosEntryModeTypes _CredentialsOnFile = new PosEntryModeTypes(10);
    private static readonly PosEntryModeTypes _ManualEntryFallbackForIccOrMagstripe = new PosEntryModeTypes(79);
    private static readonly PosEntryModeTypes _MagstripeFallbackForIcc = new PosEntryModeTypes(80);
    private static readonly PosEntryModeTypes _ManualEntryForEcommerce = new PosEntryModeTypes(81);
    private static readonly PosEntryModeTypes _TrackData = new PosEntryModeTypes(90);
    private static readonly PosEntryModeTypes _Token = new PosEntryModeTypes(91);
    private static readonly PosEntryModeTypes _IccWithoutCvv = new PosEntryModeTypes(95);

    #endregion

    #region Constructor

    public PosEntryModeTypes(byte value) : base(value)
    { }

    #endregion

    public static class MagstripeModes
    {
        #region Static Metadata

        public static readonly PosEntryModeTypes Magstripe = _Magstripe;

        /// <summary>
        ///     The terminal automates the extraction of Track 2 data after reading the Magstripe
        /// </summary>
        public static readonly PosEntryModeTypes TrackData = _TrackData;

        /// <summary>
        ///     Final fallback mode used when the other EMV Modes are unavailable
        /// </summary>
        public static readonly PosEntryModeTypes ManualEntryFallback = _ManualEntryFallbackForIccOrMagstripe;

        #endregion
    }

    public static class EmvModes
    {
        #region Static Metadata

        public static readonly PosEntryModeTypes Contact = _Icc;

        /// <summary>
        ///     Contactless EMV mode. The terminal interacts with the Proximity Integrated Circuit Card (PICC) through a Proximity
        ///     Coupling Device
        /// </summary>
        public static readonly PosEntryModeTypes Contactless = _Picc;

        /// <summary>
        ///     Fallback mode used when the EMV data might be considered unreliable, such as not including the CVV value in the
        ///     Track2 equivalent data
        /// </summary>
        public static readonly PosEntryModeTypes ContactWithoutCvvFallback = _IccWithoutCvv;

        /// <summary>
        ///     Fallback mode used when either a Contact or Contactless EMV Mode has been attempted
        /// </summary>
        public static readonly PosEntryModeTypes MagstripeFallback = _MagstripeFallbackForIcc;

        /// <summary>
        ///     Final fallback mode used when the other EMV Modes are unavailable
        /// </summary>
        public static readonly PosEntryModeTypes ManualEntryFallback = _ManualEntryFallbackForIccOrMagstripe;

        #endregion
    }

    public static class StoredValueModes
    {
        #region Static Metadata

        /// <summary>
        ///     Account number auto entry via contactless magnetic stripe. Examples of magnetic stripe contactless payment systems
        ///     include Google Wallet and Apple Pay. In these systems card data (replaced by a token due to security and PCI
        ///     compliance considerations) is injected into a mobile device.
        /// </summary>
        public static readonly PosEntryModeTypes Token = _Token;

        /// <summary>
        ///     The Cardholder's credentials are stored on file for this entry mode. Either the merchant uses a token or has the
        ///     PAN saved from a previous transaction attempt
        /// </summary>
        public static readonly PosEntryModeTypes OnFile = _CredentialsOnFile;

        #endregion
    }

    public static class EcommerceModes
    {
        #region Static Metadata

        public static readonly PosEntryModeTypes Ecommerce = _ManualEntryForEcommerce;

        #endregion
    }

    public static class OpticalModes
    {
        #region Static Metadata

        public static readonly PosEntryModeTypes QrCode = _OpticalCharacterRecognition;
        public static readonly PosEntryModeTypes Barcode = _Barcode;

        #endregion
    }
}