using Play.Interchange.Exceptions;

namespace Play.Interchange.Messages.DataFields;

public abstract class FixedLengthDataFieldMapper : DataFieldMapper
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

    #endregion
}