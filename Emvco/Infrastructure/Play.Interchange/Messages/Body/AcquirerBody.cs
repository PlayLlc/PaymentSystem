using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Core.Extensions;
using Play.Interchange.DataFields.ValueObjects;

namespace Play.Interchange.Messages.Body;

public record AcquirerBody
{
    #region Instance Values

    private readonly byte[] _Value;
    private readonly PrimaryBitmap _Primary;
    public int Count => _Value.Length;

    #endregion

    #region Constructor

    public AcquirerBody(ReadOnlySpan<byte> value)
    {
        _Primary = new PrimaryBitmap(value[..8]);
        _Value = value.ToArray();
    }

    #endregion

    #region Instance Members

    // HACK: Too slow and shit

    public bool TryGetSecondaryBitmap(out SecondaryBitmap? result)
    {
        if (_Value[7].IsBitSet(Bits.Eight))
        {
            result = new SecondaryBitmap(_Value[8..16].AsSpan());

            return true;
        }

        result = null;

        return false;
    }

    public PrimaryBitmap GetPrimaryBitmap() => new(_Value[..8]);

    public void CopyTo(Span<byte> buffer, int offset)
    {
        SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(Count);

        lock (_Value)
        {
            for (int i = 0, j = offset; i < Count; i++, j++)
                buffer[j] = _Value.ElementAt(j);
        }
    }

    #endregion
}