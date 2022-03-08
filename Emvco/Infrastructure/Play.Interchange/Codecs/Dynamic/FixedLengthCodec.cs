using Play.Interchange.Exceptions;

namespace Play.Interchange.Codecs.Dynamic;

internal abstract class FixedLengthCodec : DataFieldCodec
{
    #region Instance Members

    protected abstract ushort GetByteLength();

    /// <summary>
    /// Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public void Encode(ReadOnlySpan<byte> value, ICollection<byte> buffer)
    {
        Check.DataField.ForExactLength(value, GetByteLength(), GetDataFieldId());

        foreach (byte octet in value)
            buffer.Add(octet);
    }

    #endregion

    #region Serialization

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public byte[] Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.ForExactLength(value, GetByteLength(), GetDataFieldId());

        return value.ToArray();
    }

    /// <summary>
    /// Decode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="concreteMapper"></param>
    /// <returns></returns>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public T Decode<T>(ReadOnlySpan<byte> value, IMapDataFieldToConcreteType concreteMapper)
    {
        Check.DataField.ForMaximumLength(value, GetByteLength(), GetDataFieldId());

        return concreteMapper.Decode<T>(value);
    }

    #endregion
}