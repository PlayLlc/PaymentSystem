﻿using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     PunatcTrack2 stands for 'Position Of Unpredictable Number And Application Transaction Counter (Track2)'.
///     PUNATC(Track2) indicates to the Kernel the positions in the discretionary data field of Track 2 Data where the
///     Unpredictable Number (NumericCodec) digits and Application Transaction Counter digits have to be copied.
/// </summary>
public record PunatcTrack2 : DataElement<ushort>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F66;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public PunatcTrack2(ushort value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static PunatcTrack2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="System.Exception"></exception>
    public static PunatcTrack2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _ByteLength, Tag);

        DecodedResult<ushort> result = _Codec.Decode(EncodingId, value).ToUInt16Result() ?? throw new DataObjectParsingException(EncodingId);

        return new PunatcTrack2(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion
}