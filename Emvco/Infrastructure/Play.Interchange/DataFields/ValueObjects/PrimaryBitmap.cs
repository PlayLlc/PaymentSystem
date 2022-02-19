namespace Play.Interchange.DataFields.ValueObjects;

public record PrimaryBitmap : Bitmap
{
    #region Constructor

    public PrimaryBitmap(ulong value) : base(value)
    { }

    public PrimaryBitmap(ReadOnlySpan<byte> value) : base(value)
    { }

    #endregion
}