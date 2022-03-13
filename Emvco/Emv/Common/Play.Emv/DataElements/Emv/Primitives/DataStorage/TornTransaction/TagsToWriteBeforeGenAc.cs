using System;
using System.Collections.Generic;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     List of data objects indicating the Terminal data writing  requests to be sent to the Card before processing the
///     GENERATE AC command or the RECOVER AC command. This data object may be provided several times by the  Terminal in a
///     DET Signal. Therefore, these values must be  accumulated in Tags To Write Yet Before Gen AC buffer.
/// </summary>
public record TagsToWriteBeforeGenAc : DataExchangeResponse, IEqualityComparer<TagsToWriteBeforeGenAc>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8102;

    #endregion

    #region Constructor

    public TagsToWriteBeforeGenAc(params TagLengthValue[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static TagsToWriteBeforeGenAc Decode(ReadOnlyMemory<byte> value) =>
        new TagsToWriteBeforeGenAc(_Codec.DecodeTagLengthValues(value));

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static TagsToWriteBeforeGenAc Decode(ReadOnlySpan<byte> value) => new TagsToWriteBeforeGenAc(_Codec.DecodeTagLengthValues(value));

    #endregion

    #region Equality

    public bool Equals(TagsToWriteBeforeGenAc? x, TagsToWriteBeforeGenAc? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToWriteBeforeGenAc obj) => obj.GetHashCode();

    #endregion
}