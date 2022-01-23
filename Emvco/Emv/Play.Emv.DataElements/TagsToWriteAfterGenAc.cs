﻿using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Emv.Codecs;
using Play.Ber.Emv.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the Terminal data writing requests to be sent to the Card after processing the GENERATE AC command or th
///     RECOVER AC command.The value of this data object is composed of a series of TLVs. This data object may be provided
///     several times by the Terminal in a DET Signal.Therefore, these values must be accumulated in Tags To WriteYet After
///     Gen AC.
/// </summary>
public record TagsToWriteAfterGenAc : DataExchangeResponse, IEqualityComparer<TagsToWriteAfterGenAc>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0xFF8103;

    #endregion

    #region Constructor

    public TagsToWriteAfterGenAc(params TagLengthValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static TagsToWriteAfterGenAc Decode(ReadOnlyMemory<byte> value) => new(_Codec.DecodeTagLengthValues(value));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TagsToWriteAfterGenAc Decode(ReadOnlySpan<byte> value) => new(_Codec.DecodeTagLengthValues(value));

    #endregion

    #region Equality

    public bool Equals(TagsToWriteAfterGenAc? x, TagsToWriteAfterGenAc? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToWriteAfterGenAc obj) => obj.GetHashCode();

    #endregion
}