﻿using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Information reported by the Kernel to the Terminal, about the  processing of PUT DATA commands before sending the
///     GENERATE AC command. Possible values are 'completed' or 'not completed'. In the latter  case, this status is not
///     specific about which of the PUT DATA commands failed, or about how many of these commands  have failed or
///     succeeded. This data object is part of the Discretionary Data provided by  the Kernel to the Terminal.
/// </summary>
public record PreGenAcPutDataStatus : DataElement<byte>, IEqualityComparer<PreGenAcPutDataStatus>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF810F;
    private static readonly PreGenAcPutDataStatus _DefaultComplete = new(0x80);
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public PreGenAcPutDataStatus(byte value) : base(value)
    { }

    #endregion

    #region Serialization

    public override PreGenAcPutDataStatus Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    public static PreGenAcPutDataStatus Decode(ReadOnlyMemory<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new PreGenAcPutDataStatus(value.Span[0]);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
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

    #region Instance Members

    public static PreGenAcPutDataStatus GetDefaultCompletedPreGenAcPutDataStatus() => _DefaultComplete;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsComplete() => _Value.IsBitSet(Bits.Eight);

    #endregion
}