using Microsoft.Toolkit.HighPerformance.Buffers;

namespace Play.Interchange.Messages.DataFields;

public readonly ref struct DataField
{
    #region Instance Values

    private readonly Span<byte> _Value;
    private readonly DataFieldIdTypes _DataFieldId;

    #endregion

    #region Constructor

    public DataField(DataFieldIdTypes dataFieldType, ReadOnlySpan<byte> value)
    {
        _DataFieldId = dataFieldType;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);

        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        _Value = buffer;
    }

    #endregion

    #region Instance Members

    public byte GetDataFieldId() => _DataFieldId;
    public Span<byte> AsSpan() => _Value;

    public void CopyTo(List<byte> buffer)
    {
        for (int i = 0; i < _Value.Length; i++)
            buffer.Add(_Value[i]);
    }

    #endregion
}