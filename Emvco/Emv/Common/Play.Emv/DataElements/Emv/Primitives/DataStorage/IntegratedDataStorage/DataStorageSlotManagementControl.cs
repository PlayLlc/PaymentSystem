using System;
using System.Numerics;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the Card indication, obtained in the response to the GET PROCESSING OPTIONS command, about the status of
///     the slot containing data associated to the DS Requested Operator ID.
/// </summary>
public record DataStorageSlotManagementControl : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F6F;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageSlotManagementControl(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public bool IsPermanent() => _Value.IsBitSet(Bits.Eight);
    public bool IsVolatile() => _Value.IsBitSet(Bits.Seven);
    public bool IsLowVolatility() => _Value.IsBitSet(Bits.Six);
    public bool IsLocked() => _Value.IsBitSet(Bits.Five);
    public bool IsDeactivated() => _Value.IsBitSet(Bits.One);
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageSlotManagementControl Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageSlotManagementControl Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(EncodingId, value) as DecodedResult<byte>
            ?? throw new DataElementParsingException(
                $"The {nameof(DataStorageSlotManagementControl)} could not be initialized because the {nameof(NumericCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new DataStorageSlotManagementControl(result.Value);
    }

    #endregion
}