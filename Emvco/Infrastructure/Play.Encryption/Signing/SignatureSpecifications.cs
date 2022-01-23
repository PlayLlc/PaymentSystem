namespace Play.Encryption.Encryption.Signing;

internal static class SignatureSpecifications
{
    #region Static Metadata

    public const byte HashLength = 20;
    public const byte LeadingByte = 0x6A;
    public const byte TrailingByte = 0xBC;

    #endregion
}