using Play.Codecs;
using Play.Core.Extensions;

namespace Play.Interchange.DataFields.ValueObjects;

public record Bitmap
{
    #region Static Metadata

    public const byte MaxValue = 64;

    #endregion

    #region Instance Values

    private readonly ulong _Value;

    #endregion

    #region Constructor

    public Bitmap(ulong value)
    {
        _Value = value;
    }

    public Bitmap(ReadOnlySpan<byte> value)
    {
        _Value = PlayEncoding.UnsignedInteger.GetUInt64(value);
    }

    #endregion

    #region Instance Members

    public bool IsPresent(DataFieldId dataFieldId) => _Value.IsBitSet((byte) dataFieldId);

    #endregion
}