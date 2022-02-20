using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Interchange.Messages.DataFields;

/// <summary>
///     This is used internally during encoding to temporarily store the values of a data field before serializing the
///     message as a whole
/// </summary>
internal readonly ref struct DataField
{
    #region Instance Values

    private readonly Span<byte> _Value;
    private readonly DataFieldId _DataFieldId;

    #endregion

    #region Constructor

    public DataField(DataFieldId dataFieldType, ReadOnlySpan<byte> value)
    {
        _DataFieldId = dataFieldType;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);

        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        _Value = buffer;
    }

    #endregion

    #region Instance Members

    public DataFieldId GetDataFieldId() => _DataFieldId;
    public Span<byte> AsSpan() => _Value;

    public void CopyTo(List<byte> buffer)
    {
        for (int i = 0; i < _Value.Length; i++)
            buffer.Add(_Value[i]);
    }

    #endregion
}