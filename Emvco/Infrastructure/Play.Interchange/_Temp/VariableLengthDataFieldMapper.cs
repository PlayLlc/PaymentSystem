using Play.Codecs;
using Play.Interchange.Exceptions;

namespace Play.Interchange.Messages.DataFields;

public abstract class VariableLengthDataFieldMapper : DataFieldMapper
{
    #region Instance Members

    protected abstract ushort GetMaxByteLength();
    protected abstract ushort GetLeadingOctetLength();

    public void Encode(ReadOnlySpan<byte> value, ICollection<byte> buffer)
    {
        Check.DataField.ForMaximumLength(value, GetMaxByteLength(), GetDataFieldId());
        Span<byte> leadingOctets = stackalloc byte[GetLeadingOctetLength()];
        PlayEncoding.Numeric.GetBytes(value.Length, GetLeadingOctetLength()).CopyTo(leadingOctets);
        foreach (byte octet in leadingOctets)
            buffer.Add(octet);
        foreach (byte octet in value)
            buffer.Add(octet);
    }

    #endregion

    #region Serialization

    public byte[] Decode(ReadOnlySpan<byte> value)
    {
        Check.DataField.ForMaximumLength(value, GetMaxByteLength(), GetDataFieldId());

        return value[GetLeadingOctetLength()..].ToArray();
    }

    public T Decode<T>(ReadOnlySpan<byte> value, IMapDataFieldToConcreteType concreteMapper)
    {
        Check.DataField.ForMaximumLength(value, GetMaxByteLength(), GetDataFieldId());

        return concreteMapper.Decode<T>(value);
    }

    #endregion
}