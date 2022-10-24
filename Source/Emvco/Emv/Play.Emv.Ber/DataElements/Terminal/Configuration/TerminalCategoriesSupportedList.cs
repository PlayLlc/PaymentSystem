using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Specifications;
using Play.Emv.Ber.Enums;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Contains a list of one or more <see cref="TerminalCategoryCodes" /> supported by the terminal
/// </summary>
public record TerminalCategoriesSupportedList : DataElement<BigInteger>, IEqualityComparer<TerminalCategoriesSupportedList>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F3E;

    #endregion

    #region Constructor

    public TerminalCategoriesSupportedList(BigInteger value) : base(value)
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalCategoriesSupportedList Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalCategoriesSupportedList Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalCategoriesSupportedList Decode(ReadOnlySpan<byte> value)
    {
        BigInteger result = PlayCodec.BinaryCodec.DecodeToBigInteger(value);

        return new TerminalCategoriesSupportedList(result);
    }

    public override byte[] EncodeValue(BerCodec codec) => PlayCodec.BinaryCodec.Encode(_Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => PlayCodec.BinaryCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalCategoriesSupportedList? x, TerminalCategoriesSupportedList? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalCategoriesSupportedList obj) => obj.GetHashCode();

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    public TerminalCategoryCodes[] GetTerminalCategoryCodes()
    {
        if (_Value == 0)
            return Array.Empty<TerminalCategoryCodes>();

        byte[] buffer = _Value.ToByteArray(true);

        TerminalCategoryCodes[] result = (buffer.Length % Specs.Integer.UInt16.ByteCount) == 0
            ? new TerminalCategoryCodes[buffer.Length / Specs.Integer.UInt16.ByteCount]
            : new TerminalCategoryCodes[(buffer.Length / Specs.Integer.UInt16.ByteCount) + 1];

        for (int i = 0, j = 0; i < result.Length ; i++)
        {
            ushort categoryCode = PlayCodec.BinaryCodec.DecodeToUInt16(buffer[j..(j + 2)]);
            result[i] = TerminalCategoryCodes.Get((ushort)categoryCode);
            j+=2;
        }

        return result;
    }

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool IsPointOfInteractionApduCommandRequested() => GetTerminalCategoryCodes().Any(a => a == TerminalCategoryCodes.TransitGate);

    #endregion
}