using System;
using System.Collections.Generic;
using System.Linq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Description: List of tags indicating the data the Terminal has requested to be read. This data item is present if
///     the Terminal wants any data back from the Card before the Data Record. This could be in the context of SDS, or for
///     non data storage usage reasons, for example the PAN. This data item may contain configured data. This data object
///     may be provided several times by the Terminal. Therefore, the values of each of these tags must be accumulated in
///     the Tags To Read Yet buffer. A.1.154 Tags To Read Yet
/// </summary>
public record TagsToRead : DataExchangeRequest, IEqualityComparer<TagsToRead>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BerEncodingIdType.VariableCodec;
    public static readonly Tag Tag = 0xDF8112;

    #endregion

    #region Constructor

    public TagsToRead(Tag[] tags) : base(tags)
    { }

    public TagsToRead() : base(Array.Empty<Tag>())
    { }

    #endregion

    #region Instance Members

    public override Tag GetTag() => Tag;
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public Tag[] AsTags() => _Value.ToArray();

    /// <summary>
    ///     Checks the Tags of the <see cref="TagLengthValue" /> array provided in the argument and removes any matching tags
    ///     from this queue
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public int Resolve(TagLengthValue[] value)
    {
        for (nint i = 0; i < _Value.Count; i++)
        {
            if (!TryDequeue(out Tag result))
                throw new DataElementParsingException($"The {nameof(TagsToRead)} could not dequeue a value from memory");

            if (value.All(a => a.GetTag() != result))
                Enqueue(result);
        }

        return _Value.Count;
    }

    public void Resolve(TagLengthValue value)
    {
        if (!TryPeek(out Tag firstTag))
            throw new DataElementParsingException($"The {nameof(TagsToRead)} could not dequeue a value from memory");

        if (value.GetTag() == firstTag)
        {
            _Value.Dequeue();

            return;
        }

        for (nint i = 0; i < _Value.Count; i++)
        {
            if (!TryDequeue(out Tag currentTag))
                throw new DataElementParsingException($"The {nameof(TagsToRead)} could not dequeue a value from memory");

            if (value.GetTag() == currentTag)
                return;

            Enqueue(currentTag);
        }
    }

    public Tag[] GetTagsToReadYet() => _Value.ToArray();

    #endregion

    #region Serialization

    /// <summary>
    ///     Decode
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public static TagsToRead Decode(ReadOnlyMemory<byte> value)
    {
        if (value.IsEmpty)
            return new TagsToRead();

        return new TagsToRead(_Codec.DecodeTagSequence(value.Span));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static TagsToRead Decode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            return new TagsToRead();

        // This Value field is already BER encoded, which is what this object's _Value field requires
        return new TagsToRead(_Codec.DecodeTagSequence(value));
    }

    #endregion

    #region Equality

    public bool Equals(TagsToRead? x, TagsToRead? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToRead obj) => obj.GetHashCode();

    #endregion
}