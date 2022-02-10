using Play.Interchange.Messages.DataFields;

namespace Play.Interchange.Messages.Header;

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