using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Interchange.Messages.Body;
using Play.Interchange.Messages.Header;

namespace Play.Interchange.Messages;

internal class AcquirerMessage
{
    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public AcquirerMessage(AcquirerHeader header, AcquirerBody body)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(GetByteCount(body));
        Span<byte> buffer = spanOwner.Span;

        header.CopyTo(buffer, 0);
        body.CopyTo(buffer, AcquirerHeader.Length);
        _Value = buffer.ToArray();
    }

    #endregion

    #region Instance Members

    private int GetByteCount(AcquirerBody body) => AcquirerHeader.Length + body.Count;
    public byte[] GetBytes() => _Value;

    #endregion
}