﻿using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Identifiers;

namespace Play.Ber.DataObjects;

public record Null : PrimitiveValue, IEqualityComparer<Null>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = OctetStringBerCodec.Identifier;
    public static readonly uint Tag = 0x5;

    #endregion

    #region Instance Members

    public byte[] AsRawTlv() => new byte[2] {(byte) Tag, 0x00};
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => 0;

    #endregion

    #region Serialization

    public override byte[] EncodeValue(BerCodec codec) => Array.Empty<byte>();
    public override byte[] EncodeValue(BerCodec codec, int length) => Array.Empty<byte>();
    public new byte[] EncodeTagLengthValue(BerCodec codec, int length) => Array.Empty<byte>();
    public new byte[] EncodeTagLengthValue(BerCodec codec) => Array.Empty<byte>();

    #endregion

    #region Equality

    public bool Equals(Null? x, Null? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(Null obj) => obj.GetHashCode();

    #endregion
}