﻿using Play.Codecs;
using Play.Interchange.Exceptions;

namespace Play.Interchange.Codecs.Dynamic;

internal abstract class VariableLengthCodec : DataFieldCodec
{
    #region Instance Members

    protected abstract ushort GetMaxByteLength();
    protected abstract ushort GetLeadingOctetLength();

    /// <summary>
    /// Encode
    /// </summary>
    /// <param name="value"></param>
    /// <param name="buffer"></param>
    /// <exception cref="InterchangeDataFieldOutOfRangeException"></exception>
    public void Encode(ReadOnlySpan<byte> value, ICollection<byte> buffer)
    {
        Check.DataField.ForMaximumLength(value, GetMaxByteLength(), GetDataFieldId());
        Span<byte> leadingOctets = stackalloc byte[GetLeadingOctetLength()];
        PlayCodec.NumericCodec.Encode(value.Length, GetLeadingOctetLength()).CopyTo(leadingOctets);
        foreach (byte octet in leadingOctets)
            buffer.Add(octet);
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
        Check.DataField.ForMaximumLength(value, GetMaxByteLength(), GetDataFieldId());

        return value[GetLeadingOctetLength()..].ToArray();
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
        Check.DataField.ForMaximumLength(value, GetMaxByteLength(), GetDataFieldId());

        return concreteMapper.Decode<T>(value);
    }

    #endregion
}