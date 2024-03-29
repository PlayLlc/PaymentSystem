﻿using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Codecs;

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

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
        }

        return codec!.Encode(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T[] value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
        }

        return codec!.Encode(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T value, int length) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
        }

        return codec!.Encode(value, length);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    internal byte[] Encode<T>(PlayEncodingId playEncodingId, T[] value, int length) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
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
    /// <exception cref="BerParsingException"></exception>
    internal DecodedMetadata Decode(PlayEncodingId playEncodingId, ReadOnlySpan<byte> value)
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
        }

        return codec!.Decode(value);
    }

    internal byte[] EncodeValue(PlayEncodingId encodingId, TagLength[] value)
    {
        return value.SelectMany(a => a.Encode()).ToArray();
    }

    internal byte[] EncodeValue(PlayEncodingId encodingId, TagLengthValue[] value)
    {
        return value.SelectMany(a => a.EncodeValue()).ToArray();
    }

    #endregion

    #region Byte Count

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
        }

        return codec.GetByteCount(value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public ushort GetByteCount<T>(PlayEncodingId playEncodingId, T[] value) where T : struct
    {
        if (!_BerPrimitiveCodecMap.TryGetValue(playEncodingId, out PlayCodec? codec))
        {
            throw new BerParsingException(
                $"The value could not be decoded because there is not a {nameof(PlayCodec)} configured with the {nameof(PlayEncodingId)}: {playEncodingId}");
        }

        return _BerPrimitiveCodecMap[playEncodingId].GetByteCount(value);
    }

    #endregion
}