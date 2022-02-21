using Play.Interchange.Exceptions;

namespace Play.Interchange.Codecs.Dynamic;

internal abstract class FixedLengthCodec : DataFieldCodec
{
    #region Instance Members

    protected abstract ushort GetByteLength();

    public void Encode(ReadOnlySpan<byte> value, ICollection<byte> buffer)
    {
        Check.DataField.ForExactLength(value, GetByteLength(), GetDataFieldId());

        foreach (byte octet in value)
            buffer.Add(octet);
    }

    #endregion

    #region Serialization

    public byte[] Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.ForExactLength(value, GetByteLength(), GetDataFieldId());

        return value.ToArray();
    }

    public T Decode<T>(ReadOnlySpan<byte> value, IMapDataFieldToConcreteType concreteMapper)
    {
        Check.DataField.ForMaximumLength(value, GetByteLength(), GetDataFieldId());

        return concreteMapper.Decode<T>(value);
    }

    #endregion
}