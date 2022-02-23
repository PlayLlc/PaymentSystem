using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Codecs;

using PlayCodec = Play.Ber.Codecs.PlayCodec;

namespace Play.Ber.InternalFactories;

internal sealed class ValueFactory
{
    #region Instance Values

    private readonly IDictionary<PlayEncodingId, PlayCodec> _BerPrimitiveCodecMap;

    #endregion

    #region Constructor

    public ValueFactory(IDictionary<PlayEncodingId, PlayCodec> berPrimitiveCodecMap)
    {
        _BerPrimitiveCodecMap = berPrimitiveCodecMap;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="playEncodingId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerInternalException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return codec.GetByteCount(value);
    }

    /// <summary>
    ///     GetByteCount
    /// </summary>
    /// <param name="playEncodingId"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T[] value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return _BerPrimitiveCodecMap[playEncodingId].GetByteCount(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return codec!.Encode(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T[] value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return codec!.Encode(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T value, int length) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return codec!.Encode(value, length);
    }

    /// <exception cref="InvalidOperationException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T[] value, int length) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerConfigurationException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return codec!.Encode(value, length);
    }

    #endregion

    #region Serialization

    /// <summary>
    ///     The <see cref="PlayEncodingId" /> is used to resolve the correct <see cref="PlayCodec" /> so the byte
    ///     sequence provided can be
    ///     decoded according to the format requirements of that codec
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public DecodedMetadata Decode(PlayEncodingId playEncodingId, ReadOnlySpan<byte> value)
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new InvalidOperationException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the Fully Qualified Name: {playEncodingId.GetFullyQualifiedName()} and with Id: [{playEncodingId}]");
        }

        return codec!.Decode(value);
    }

    #endregion
}