﻿using System;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Contains the Card indication, obtained in the response to the GET PROCESSING OPTIONS command, about the slot
///     type(s) available for data storage.
/// </summary>
public record DataStorageSlotAvailability : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F5F;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageSlotAvailability(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public bool IsPermanentSlotTypeSet() => _Value.IsBitSet(Bits.Eight);
    public bool IsVolatileSlotTypeSet() => _Value.IsBitSet(Bits.Seven);

    #endregion

    #region Serialization

    public static DataStorageSlotAvailability Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static DataStorageSlotAvailability Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new DataStorageSlotAvailability(value[0]);
    }

    #endregion
}