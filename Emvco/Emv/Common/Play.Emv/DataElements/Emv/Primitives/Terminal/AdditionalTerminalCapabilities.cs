using System;
using System.Collections.Generic;

using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.Terminal;

/// <summary>
///     Description: Indicates the data input and output capabilities of the Terminal and Reader.
/// </summary>
public record AdditionalTerminalCapabilities : DataElement<ulong>, IEqualityComparer<AdditionalTerminalCapabilities>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F40;
    private const byte _ByteLength = 5;

    #endregion

    #region Constructor

    public AdditionalTerminalCapabilities(ulong value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool Administrative() => _Value.IsBitSet(33);
    public bool AlphabeticalAndSpecialCharactersKeys() => _Value.IsBitSet(23);
    public bool Cash() => _Value.IsBitSet(40);
    public bool Cashback() => _Value.IsBitSet(47);
    public bool CashDeposit() => _Value.IsBitSet(32);
    public bool CodeTable1() => _Value.IsBitSet(1);
    public bool CodeTable10() => _Value.IsBitSet(10);
    public bool CodeTable2() => _Value.IsBitSet(2);
    public bool CodeTable3() => _Value.IsBitSet(3);
    public bool CodeTable4() => _Value.IsBitSet(4);
    public bool CodeTable5() => _Value.IsBitSet(5);
    public bool CodeTable6() => _Value.IsBitSet(6);
    public bool CodeTable7() => _Value.IsBitSet(7);
    public bool CodeTable8() => _Value.IsBitSet(8);
    public bool CodeTable9() => _Value.IsBitSet(9);
    public bool CommandKeys() => _Value.IsBitSet(22);
    public AdditionalTerminalCapabilities CreateValueCopy(ref AdditionalTerminalCapabilities other) => throw new NotImplementedException();
    public bool DisplayAttendant() => _Value.IsBitSet(14);
    public bool DisplayCardholder() => _Value.IsBitSet(13);
    public bool FunctionKeys() => _Value.IsBitSet(21);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public bool Goods() => _Value.IsBitSet(39);
    public bool Inquiry() => _Value.IsBitSet(36);
    public bool NumericKeys() => _Value.IsBitSet(24);
    public bool Payment() => _Value.IsBitSet(34);
    public bool PrintAttendant() => _Value.IsBitSet(16);
    public bool PrintCardholder() => _Value.IsBitSet(15);
    public bool Services() => _Value.IsBitSet(38);
    public bool Transfer() => _Value.IsBitSet(35);

    #endregion

    #region Serialization

    public static AdditionalTerminalCapabilities Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static AdditionalTerminalCapabilities Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value) as DecodedResult<ulong>
            ?? throw new InvalidOperationException(
                $"The {nameof(AdditionalTerminalCapabilities)} could not be initialized because the {nameof(BinaryCodec)} returned a null {nameof(DecodedResult<ulong>)}");

        return new AdditionalTerminalCapabilities(result.Value);
    }

    public new byte[] EncodeValue() => _Codec.EncodeValue(EncodingId, _Value, _ByteLength);

    #endregion

    #region Equality

    public bool Equals(AdditionalTerminalCapabilities? x, AdditionalTerminalCapabilities? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(AdditionalTerminalCapabilities obj) => obj.GetHashCode();

    #endregion
}