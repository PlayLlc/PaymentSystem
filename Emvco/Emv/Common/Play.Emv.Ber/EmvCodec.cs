using Microsoft.Toolkit.HighPerformance.Buffers;

using Play.Ber;
using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Codecs;
using Play.Core.Specifications;
using Play.Emv.Ber.Codecs;

using PlayCodec = Play.Codecs.PlayCodec;

namespace Play.Emv.Ber;

public class EmvCodec : BerCodec
{
    #region Static Metadata

    public static readonly BerConfiguration Configuration = new(new Dictionary<PlayEncodingId, PlayCodec>
    {
        {AlphabeticCodec.EncodingId, new AlphabeticCodec()},
        {AlphaNumericCodec.EncodingId, new AlphaNumericCodec()},
        {AlphaNumericSpecialCodec.EncodingId, new AlphaNumericSpecialCodec()},
        {CompressedNumericCodec.EncodingId, new CompressedNumericCodec()},
        {NumericCodec.EncodingId, new NumericCodec()},
        {BinaryCodec.EncodingId, new BinaryCodec()},
        {BerEncodingIdType.VariableCodec, new HexadecimalCodec()}
    });

    private static readonly EmvCodec _Codec = new();

    #endregion

    #region Constructor

    public EmvCodec() : base(Configuration)
    { }

    #endregion

    #region Instance Members

    public static EmvCodec GetBerCodec() => _Codec;

    /// <summary>
    ///     Parses a sequence of metadata containing concatenated Tag-Length values and returns an array
    ///     of the Tag-Length pairs.
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="InternalEmvEncodingException"></exception>
    /// <remarks>
    ///     This method expects that the argument provided will only contain Tag-Length pairs. It will
    ///     not handle a sequence of Tag-Length-Value
    /// </remarks>
    public TagLength[] DecodeTagLengthPairs(ReadOnlySpan<byte> value)
    {
        SpanOwner<TagLength> spanOwner = SpanOwner<TagLength>.Allocate(value.Length / 2);
        Span<TagLength> buffer = spanOwner.Span;

        int bufferOffset = 0;

        for (int j = 0; j < value.Length; bufferOffset++)
        {
            buffer[bufferOffset] = DecodeTagLength(value[j..]);

            j += buffer[bufferOffset].GetTag().GetByteCount() + buffer[bufferOffset].GetLength().GetByteCount();
        }

        return buffer[..bufferOffset].ToArray();
    }

    public bool IsTagPresent(Tag tagToSearch, ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length;)
        {
            Tag currentTag = new(value[i..]);

            if (currentTag == tagToSearch)
                return true;

            i += currentTag.GetByteCount();
        }

        return false;
    }

    /// <summary>
    ///     Parses a sequence of metadata containing a concatenation of Tag identifiers and returns an array of Tags.
    /// </summary>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <remarks>
    ///     This method expects that the argument provided will only contain Tag-Length pairs. It will
    ///     not handle a sequence of Tag-Length-Value
    /// </remarks>
    public Tag[] DecodeTagSequence(ReadOnlySpan<byte> value)
    {
        if (value.Length < Specs.ByteArray.StackAllocateCeiling)
        {
            Span<Tag> buffer = stackalloc Tag[value.Length];
            int i = 0;

            for (int j = 0; j < value.Length; i++)
            {
                Tag tempValue = new(value[j..]);
                buffer[i] = tempValue;
                j += tempValue.GetByteCount();
            }

            return buffer[..i].ToArray();
        }
        else
        {
            using SpanOwner<Tag> spanOwner = SpanOwner<Tag>.Allocate(value.Length);
            Span<Tag> buffer = spanOwner.Span;
            int i = 0;

            for (int j = 0; j < value.Length; i++)
            {
                Tag tempValue = new(value[j..]);
                buffer[i] = tempValue;
                j += tempValue.GetByteCount();
            }

            return buffer[..i].ToArray();
        }
    }

    #endregion
}