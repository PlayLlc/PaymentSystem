﻿using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.InternalFactories;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains the Terminal data writing requests to be sent to the Card after processing the GENERATE AC command or th
///     RECOVER AC command.The value of this data object is composed of a series of TLVs. This data object may be provided
///     several times by the Terminal in a DET Signal.Therefore, these values must be accumulated in Tags To WriteYet After
///     Gen AC.
/// </summary>
public record TagsToWriteAfterGeneratingApplicationCryptogram : DataExchangeResponse, IEqualityComparer<TagsToWriteAfterGeneratingApplicationCryptogram>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xFF8103;

    #endregion

    #region Constructor

    public TagsToWriteAfterGeneratingApplicationCryptogram(params PrimitiveValue[] value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TagsToWriteAfterGeneratingApplicationCryptogram Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TagsToWriteAfterGeneratingApplicationCryptogram Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TagsToWriteAfterGeneratingApplicationCryptogram Decode(ReadOnlySpan<byte> value) => new(ResolveTagsToWrite(value).ToArray());

    #endregion

    #region Equality

    public bool Equals(TagsToWriteAfterGeneratingApplicationCryptogram? x, TagsToWriteAfterGeneratingApplicationCryptogram? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TagsToWriteAfterGeneratingApplicationCryptogram obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public override ushort GetValueByteCount() => (ushort) _Value.ToArray().Sum(x => x.GetValueByteCount(_Codec));

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private static IEnumerable<PrimitiveValue> ResolveTagsToWrite(ReadOnlySpan<byte> value)
    {
        TagLengthValue[] tlv = _Codec.DecodeTagLengthValues(value).Where(a => _KnownPutDataTags.Any(b => b == a.GetTag())).ToArray();
        PrimitiveValue[] result = new PrimitiveValue[tlv.Length];

        for (int i = 0; i < tlv.Length; i++)
        {
            if (tlv[i].GetTag() == UnprotectedDataEnvelope1.Tag)
                result[i] = UnprotectedDataEnvelope1.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope2.Tag)
                result[i] = UnprotectedDataEnvelope2.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope3.Tag)
                result[i] = UnprotectedDataEnvelope3.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope4.Tag)
                result[i] = UnprotectedDataEnvelope4.Decode(tlv[i].EncodeValue(_Codec).AsSpan());

            if (tlv[i].GetTag() == UnprotectedDataEnvelope5.Tag)
                result[i] = UnprotectedDataEnvelope5.Decode(tlv[i].EncodeValue(_Codec).AsSpan());
        }

        return result;
    }

    #endregion
}