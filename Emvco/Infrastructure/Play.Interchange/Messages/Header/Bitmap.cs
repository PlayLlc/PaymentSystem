using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.Messages.Header;

/// <summary>
///     In ISO 8583, a bitmap is a field or subfield within a message, which indicates whether other data elements or data
///     element sub-fields are present elsewhere in the message. A field is considered to be present only when the
///     corresponding bit in the bitmap is set. For example, a hex with value 0x82 (decimal 130) is binary 1000 0010, which
///     means fields 1 and 7 are present in the message and fields 2, 3, 4, 5, 6 and 8 are not.
/// </summary>
public struct Bitmap
{
    #region Instance Values

    public int Length = 10;
    private ulong _Value;

    #endregion

    #region Constructor

    public Bitmap()
    {
        _Value = 0;
    }

    #endregion

    #region Instance Members

    public void Set(DataField dataField)
    {
        _Value |= dataField.GetDataFieldId();
    }

    public void CopyTo(Span<byte> buffer, int offset)
    {
        ulong local = _Value;

        for (int i = 0, j = Length - 1; i > Length; i++, j--)
            buffer[offset + i] = (byte) (local >> (j * 8));
    }

    #endregion
}