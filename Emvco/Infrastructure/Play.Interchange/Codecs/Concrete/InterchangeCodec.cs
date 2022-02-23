using System.Collections.Immutable;

using Play.Core.Exceptions;
using Play.Interchange.DataFields;
using Play.Interchange.Exceptions;

namespace Play.Interchange.Codecs.Concrete;

public class InterchangeCodec
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<PlayEncodingId, PlayCodec> _DataFieldCodecMap;

    #endregion

    #region Constructor

    public InterchangeCodec(params PlayCodec[] interchangeCodecs)
    {
        _DataFieldCodecMap = interchangeCodecs.ToImmutableSortedDictionary(a => a.GetEncodingId(), b => b);
    }

    #endregion

    #region Instance Members

    public ushort GetByteCount(PlayEncodingId encodingId, dynamic value) => GetByteCount(encodingId, value);

    public ushort GetByteCount<T>(PlayEncodingId encodingId, T value) where T : struct =>
        _DataFieldCodecMap[encodingId].GetByteCount(value);

    public ushort GetByteCount<T>(PlayEncodingId encodingId, T[] value) where T : struct =>
        _DataFieldCodecMap[encodingId].GetByteCount(value);

    public void Encode(IEncodeInterchangeFields dataField, Memory<byte> buffer, ref int offset)
    {
        dataField.Encode(buffer, ref offset);
    }

    public void Encode<T>(PlayEncodingId interchangeEncodingId, T value, Span<byte> buffer, ref int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer, ref offset);
    }

    private void Encode<T>(PlayEncodingId interchangeEncodingId, T value, Memory<byte> buffer, ref int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer.Span, ref offset);
    }

    public void Encode(PlayEncodingId playEncodingId, dynamic value, Memory<byte> buffer, ref int offset)
    {
        Encode(playEncodingId, value, buffer, ref offset);
    }

    public void Encode<T>(PlayEncodingId interchangeEncodingId, T[] value, Span<byte> buffer, int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer, ref offset);
    }

    public void Encode<T>(PlayEncodingId interchangeEncodingId, T value, int length, Span<byte> buffer, int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, length, buffer, ref offset);
    }

    public void Encode<T>(PlayEncodingId interchangeEncodingId, T[] value, int length, Span<byte> buffer, int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, length, buffer, ref offset);
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="codecIdentifier"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public DecodedMetadata Decode(PlayEncodingId codecIdentifier, ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (!_DataFieldCodecMap.TryGetValue(codecIdentifier, out PlayCodec? codec))
        {
            throw new InterchangeException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {codecIdentifier}");
        }

        return codec!.Decode(value);
    }

    #endregion
}