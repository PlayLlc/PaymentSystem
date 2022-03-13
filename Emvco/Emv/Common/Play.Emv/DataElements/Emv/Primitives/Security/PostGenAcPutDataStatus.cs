using System;
using System.Collections.Generic;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Information reported by the Kernel to the Terminal, about the  processing of PUT DATA commands after processing the
///     GENERATE AC command. Possible values are 'completed' or 'not completed'. In the latter  case, this status is not
///     specific about which of the PUT DATA commands failed, or about how many of these commands  have failed or
///     succeeded. This data object is part of the Discretionary Data provided by  the Kernel to the Terminal.
/// </summary>
public record PostGenAcPutDataStatus : DataElement<byte>, IEqualityComparer<PostGenAcPutDataStatus>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF810E;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public PostGenAcPutDataStatus(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization
    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PostGenAcPutDataStatus Decode(ReadOnlyMemory<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new PostGenAcPutDataStatus(value.Span[0]);
    }

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PostGenAcPutDataStatus Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new PostGenAcPutDataStatus(value[0]);
    }

    #endregion

    #region Equality

    public bool Equals(PostGenAcPutDataStatus? x, PostGenAcPutDataStatus? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PostGenAcPutDataStatus obj) => obj.GetHashCode();

    #endregion
}