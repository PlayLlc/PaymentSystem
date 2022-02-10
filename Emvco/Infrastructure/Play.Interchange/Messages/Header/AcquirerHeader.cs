using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Interchange.Messages.Header;

public readonly ref struct AcquirerHeader
{
    #region Static Metadata

    public const int Length = 10;

    #endregion

    #region Instance Values

    private readonly Span<byte> _Value;

    #endregion

    #region Constructor

    public AcquirerHeader(MessageTypeIndicator messageTypeIndicator, Bitmap bitmap)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(10);
        Span<byte> buffer = spanOwner.Span;

        buffer[0] = messageTypeIndicator.GetFirstByte();
        buffer[1] = messageTypeIndicator.GetSecondByte();
        bitmap.CopyTo(buffer, 2);
        _Value = buffer;
    }

    #endregion

    #region Instance Members

    public void CopyTo(Span<byte> buffer, int offset)
    {
        _Value.CopyTo(buffer[offset..]);
    }

    #endregion
}