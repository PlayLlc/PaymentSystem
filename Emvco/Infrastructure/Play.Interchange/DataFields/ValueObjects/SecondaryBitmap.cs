namespace Play.Interchange.DataFields.ValueObjects;

public record SecondaryBitmap : Bitmap
{
    #region Constructor

    public SecondaryBitmap(ulong value) : base(value)
    { }

    public SecondaryBitmap(ReadOnlySpan<byte> value) : base(value)
    { }

    #endregion
}