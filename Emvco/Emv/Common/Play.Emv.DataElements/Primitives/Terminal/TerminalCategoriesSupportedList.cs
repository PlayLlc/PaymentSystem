using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Core.Specifications;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains a list of one or more <see cref="TerminalCategoryCode" /> supported by the terminal
/// </summary>
public record TerminalCategoriesSupportedList : DataElement<BigInteger>, IEqualityComparer<TerminalCategoriesSupportedList>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = BinaryCodec.Identifier;
    public static readonly Tag Tag = 0x9F3E;

    #endregion

    #region Constructor

    public TerminalCategoriesSupportedList(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override BerEncodingId GetBerEncodingId() => BerEncodingId;
    public override Tag GetTag() => Tag;

    public TerminalCategoryCode[] GetTerminalCategoryCodes()
    {
        if (_Value == 0)
            return Array.Empty<TerminalCategoryCode>();

        BigInteger temp = _Value;
        TerminalCategoryCode[] result = (_Value.GetByteCount() % Specs.Integer.UInt16.ByteSize) == 0
            ? new TerminalCategoryCode[_Value.GetByteCount() / Specs.Integer.UInt16.ByteSize]
            : new TerminalCategoryCode[(_Value.GetByteCount() / Specs.Integer.UInt16.ByteSize) + 1];

        for (int i = 0; temp > 0; i++)
        {
            result[i] = TerminalCategoryCode.Get((ushort) temp);
            temp >>= 16;
        }

        return result;
    }

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);
    public bool IsPointOfInteractionApduCommandRequested() => GetTerminalCategoryCodes().Any(a => a == TerminalCategoryCode.TransitGate);

    #endregion

    #region Serialization

    public static TerminalCategoriesSupportedList Decode(ReadOnlyMemory<byte> value, BerCodec codec) => Decode(value.Span, codec);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalCategoriesSupportedList Decode(ReadOnlySpan<byte> value, BerCodec codec)
    {
        DecodedResult<BigInteger> result = codec.Decode(BerEncodingId, value) as DecodedResult<BigInteger>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalCategoriesSupportedList)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<BigInteger>)}");

        return new TerminalCategoriesSupportedList(result.Value);
    }

    public override byte[] EncodeValue(BerCodec codec) => codec.EncodeValue(BerEncodingId, _Value);
    public override byte[] EncodeValue(BerCodec codec, int length) => codec.EncodeValue(BerEncodingId, _Value, length);

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
}