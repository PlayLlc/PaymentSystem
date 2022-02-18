using System.Collections.Immutable;

using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs;

public class InterchangeCodec
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<InterchangeEncodingId, InterchangeDataFieldCodec> _DataFieldCodecMap;

    #endregion

    #region Constructor

    public InterchangeCodec(params InterchangeDataFieldCodec[] interchangeCodecs)
    {
        _DataFieldCodecMap = interchangeCodecs.ToImmutableSortedDictionary(a => a.GetIdentifier(), b => b);
    }

    #endregion

    #region Instance Members

    public ushort GetByteCount<T>(InterchangeEncodingId encodingId, T value) where T : struct =>
        _DataFieldCodecMap[encodingId].GetByteCount(value);

    public ushort GetByteCount<T>(InterchangeEncodingId encodingId, T[] value) where T : struct =>
        _DataFieldCodecMap[encodingId].GetByteCount(value);

    public void Encode(InterchangeDataField dataField, Span<byte> buffer, ref int offset)
    {
        dataField.Encode(this, buffer, ref offset);
    }

    public void Encode<T>(InterchangeEncodingId interchangeEncodingId, T value, Span<byte> buffer, ref int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer, ref offset);
    }

    //public void Encode<T>(InterchangeEncodingId interchangeEncodingId, T[] value, Span<byte> buffer, int offset) where T : struct
    //{
    //    _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer, ref offset);
    //}

    public void Encode<T>(InterchangeEncodingId interchangeEncodingId, T value, int length, Span<byte> buffer, int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, length, buffer, ref offset);
    }

    public void Encode<T>(InterchangeEncodingId interchangeEncodingId, T[] value, int length, Span<byte> buffer, int offset)
        where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, length, buffer, ref offset);
    }

    #endregion
}