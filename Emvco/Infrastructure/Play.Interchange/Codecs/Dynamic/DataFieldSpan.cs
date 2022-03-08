using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Codecs;
using Play.Core.Extensions;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs.Dynamic;

internal readonly ref struct DataFieldSpan
{
    #region Instance Values

    private readonly Span<byte> _Value;
    private readonly DataFieldId _DataFieldId;

    #endregion

    #region Constructor

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dataField"></param>
    /// <param name="value"></param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public DataFieldSpan(DataFieldId dataField, ReadOnlySpan<byte> value)
    {
        const ushort absoluteMaximumByteCount = 999;

        if (value.Length > absoluteMaximumByteCount)
        {
            throw new InvalidOperationException(
                $"The length of the {nameof(DataFieldSpan)} was: [{value.Length}] which exceeds the maximum allowed length of {absoluteMaximumByteCount} bytes");
        }

        _DataFieldId = dataField;

        using SpanOwner<byte> spanOwner = SpanOwner<byte>.Allocate(value.Length);

        Span<byte> buffer = spanOwner.Span;

        value.CopyTo(buffer);
        _Value = buffer;
    }

    #endregion

    #region Instance Members

    private int GetLeadingOctetByteCount()
    {
        byte numberOfDigits = _Value.Length.GetNumberOfDigits();

        return _Value.Length.GetNumberOfDigits() > 2 ? 2 : 1;
    }

    public DataFieldId GetDataFieldId() => _DataFieldId;
    public Span<byte> AsSpan() => _Value;

    private void EncodeLeadingOctets(List<byte> buffer)
    {
        ReadOnlySpan<byte> a = PlayCodec.NumericCodec.Encode(_Value.Length, GetLeadingOctetByteCount());

        for (int i = 0; i < a.Length; i++)
            buffer.Add(a[i]);
    }

    public void EncodeTo(List<byte> buffer)
    {
        EncodeLeadingOctets(buffer);

        for (int i = 0; i < _Value.Length; i++)
            buffer.Add(_Value[i]);
    }

    #endregion
}