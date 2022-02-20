using System.Collections.Immutable;

using Play.Ber.InternalFactories;
using Play.Core.Exceptions;
using Play.Emv.Interchange.Exceptions;
using Play.Interchange.DataFields;

namespace Play.Interchange.Codecs;

internal class InterchangeCodec
{
    #region Instance Values

    private readonly ImmutableSortedDictionary<InterchangeEncodingId, IInterchangeDataFieldCodec> _DataFieldCodecMap;

    #endregion

    #region Constructor

    public InterchangeCodec(params IInterchangeDataFieldCodec[] interchangeCodecs)
    {
        _DataFieldCodecMap = interchangeCodecs.ToImmutableSortedDictionary(a => a.GetIdentifier(), b => b);
    }

    #endregion

    #region Instance Members

    public ushort GetByteCount(InterchangeEncodingId encodingId, dynamic value) => GetByteCount(encodingId, value);

    public ushort GetByteCount<T>(InterchangeEncodingId encodingId, T value) where T : struct =>
        _DataFieldCodecMap[encodingId].GetByteCount(value);

    public ushort GetByteCount<T>(InterchangeEncodingId encodingId, T[] value) where T : struct =>
        _DataFieldCodecMap[encodingId].GetByteCount(value);

    public void Encode(InterchangeDataField dataField, Memory<byte> buffer, ref int offset)
    {
        dataField.Encode(buffer, ref offset);
    }

    public void Encode<T>(InterchangeEncodingId interchangeEncodingId, T value, Span<byte> buffer, ref int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer, ref offset);
    }

    private void Encode<T>(InterchangeEncodingId interchangeEncodingId, T value, Memory<byte> buffer, ref int offset) where T : struct
    {
        _DataFieldCodecMap[interchangeEncodingId].Encode(value, buffer.Span, ref offset);
    }

    public void Encode(InterchangeEncodingId interchangeEncodingId, dynamic value, Memory<byte> buffer, ref int offset)
    {
        Encode(interchangeEncodingId, value, buffer, ref offset);
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

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="codecIdentifier"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public DecodedMetadata Decode(InterchangeEncodingId codecIdentifier, ReadOnlySpan<byte> value)
    {
        CheckCore.ForEmptySequence(value, nameof(value));

        if (!_DataFieldCodecMap.TryGetValue(codecIdentifier, out IInterchangeDataFieldCodec? codec))
        {
            throw new InterchangeException(
                $"The value could not be decoded because there is not a {nameof(IInterchangeDataFieldCodec)} configured with the Fully Qualified Name: {codecIdentifier}");
        }

        return codec!.Decode(value);
    }

    #endregion
}