using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;

namespace Play.Ber.InternalFactories;

internal sealed class ValueFactory
{
    #region Instance Values

    private readonly IDictionary<BerEncodingId, BerPrimitiveCodec> _BerPrimitiveCodecMap;

    #endregion

    #region Constructor

    public ValueFactory(IDictionary<BerEncodingId, BerPrimitiveCodec> berPrimitiveCodecMap)
    {
        _BerPrimitiveCodecMap = berPrimitiveCodecMap;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="berEncodingId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ushort GetByteCount<T>(BerEncodingId berEncodingId, T value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new BerInternalException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return codec.GetByteCount(value);
    }

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="berEncodingId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ushort GetByteCount<T>(BerEncodingId berEncodingId, T[] value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return _BerPrimitiveCodecMap[berEncodingId].GetByteCount(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(BerEncodingId berEncodingId, T value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return codec!.Encode(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(BerEncodingId berEncodingId, T[] value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return codec!.Encode(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(BerEncodingId berEncodingId, T value, int length) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return codec!.Encode(value, length);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(BerEncodingId berEncodingId, T[] value, int length) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return codec!.Encode(value, length);
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     The <see cref="BerEncodingId" /> is used to resolve the correct <see cref="BerPrimitiveCodec" /> so the byte
    ///     sequence provided can be
    ///     decoded according to the format requirements of that codec
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public DecodedMetadata Decode(BerEncodingId berEncodingId, ReadOnlySpan<byte> value)
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(berEncodingId, out BerPrimitiveCodec? codec))
        {
            throw new InvalidOperationException(
                $"The value could not be decoded because there is not a {nameof(BerPrimitiveCodec)} configured with the Fully Qualified Name: {berEncodingId.GetFullyQualifiedName()} and with Id: [{berEncodingId}]");
        }

        return codec!.Decode(value);
    }

    #endregion
}