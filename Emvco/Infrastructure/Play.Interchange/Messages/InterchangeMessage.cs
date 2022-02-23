using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Interchange.Messages.Body;
using Play.Interchange.Messages.Header;

namespace Play.Interchange.Messages;

public class InterchangeMessage
{
    #region Instance Values

    private readonly byte[] _Value;

    #endregion

    #region Constructor

    public InterchangeMessage(MessageTypeIndicator messageTypeIndicator, AcquirerBody body)
    {
        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(2 + body.GetByteCount());
        Span<byte> buffer = spanOwner.Span;

        messageTypeIndicator.CopyTo(buffer);
        body.CopyTo(buffer);
        _Value = buffer.ToArray();
    }

    #endregion

    #region Instance Members

    private int GetByteCount() => _Value.Length;
    public byte[] Encode() => _Value;
    public MessageTypeIndicator GetMessageTypeIndicator() => new(PlayCodec.NumericCodec.DecodeToUInt16(_Value[..2]));

    #endregion
}