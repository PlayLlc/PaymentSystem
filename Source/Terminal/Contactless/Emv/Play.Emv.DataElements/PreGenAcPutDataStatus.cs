﻿using Play.Ber.Codecs;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Information reported by the Kernel to the Terminal, about the  processing of PUT DATA commands before sending the
///     GENERATE AC command. Possible values are 'completed' or 'not completed'. In the latter  case, this status is not
///     specific about which of the PUT DATA commands failed, or about how many of these commands  have failed or
///     succeeded. This data object is part of the Discretionary Data provided by  the Kernel to the Terminal.
/// </summary>
public record PreGenAcPutDataStatus : DataElement<byte>, IEqualityComparer<PreGenAcPutDataStatus>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0xDF810E;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public PreGenAcPutDataStatus(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static PreGenAcPutDataStatus Decode(ReadOnlyMemory<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new PreGenAcPutDataStatus(value.Span[0]);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static PreGenAcPutDataStatus Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new PreGenAcPutDataStatus(value[0]);
    }

    #endregion

    #region Equality

    public bool Equals(PreGenAcPutDataStatus? x, PreGenAcPutDataStatus? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(PreGenAcPutDataStatus obj) => obj.GetHashCode();

    #endregion
}