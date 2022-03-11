using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Emv.Primitives.DataStorage.IntegratedDataStoraged;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.DataStorage.IntegratedDataStorage;

public record DataStorageOperatorDataSetInfo : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0x9F54;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetInfo(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsPermanent() => _Value.IsBitSet(Bits.Eight);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsVolatile() => _Value.IsBitSet(Bits.Seven);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsLowVolatility() => _Value.IsBitSet(Bits.Six);

    /// <exception cref="Core.Exceptions.PlayInternalException"></exception>
    public bool IsDeclineOnDataStorageErrorSet() => _Value.IsBitSet(Bits.Four);

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetCard Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetCard Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        return new DataStorageOperatorDataSetCard(value[0]);
    }

    #endregion
}