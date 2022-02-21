using Play.Core.Extensions;

namespace Play.Interchange.DataFields.ValueObjects;

/// <summary>
///     In ISO 8583, a bitmap is a field or subfield within a message, which indicates whether other data elements or data
///     element sub-fields are present elsewhere in the message. The bitmap may be represented as 8 bytes of binary data or
///     as 16 hexadecimal characters (0–9, A–F) in the ASCII or EBCDIC character sets. A message will contain at least one
///     bitmap, called the primary bitmap, which indicates data elements 1 to 64 are present.
/// </summary>
internal class BitmapBuilder
{
    #region Static Metadata

    private const byte _ByteLength = 8;
    private const byte _MaxValue = 128;

    #endregion

    #region Instance Values

    private readonly ulong _SecondaryBitmap = 0;
    private ulong _PrimaryBitmap = 0;

    #endregion

    #region Instance Members

    public void Set(InterchangeDataField dataField)
    {
        _PrimaryBitmap = 0;

        var bitPosition = (byte) dataField.GetDataFieldId();

        if (bitPosition <= _MaxValue)
            _PrimaryBitmap.SetBit(bitPosition);
        else
            _SecondaryBitmap.SetBit((byte) (bitPosition / 64));
    }

    #endregion
}