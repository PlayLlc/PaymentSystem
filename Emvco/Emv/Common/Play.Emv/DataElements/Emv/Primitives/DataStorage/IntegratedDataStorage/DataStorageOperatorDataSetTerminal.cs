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
using Play.Emv.Ber.DataObjects;
using Play.Emv.Exceptions;

namespace Play.Emv.DataElements.Emv.Primitives.DataStorage.IntegratedDataStorage;

/// <summary>
///     Contains Terminal provided data to be forwarded to the Card with the GENERATE AC command, as per DSDOL formatting.
/// </summary>
public record DataStorageOperatorDataSetTerminal : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF63;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _MaxByteLength = 160;

    #endregion

    #region Constructor

    public DataStorageOperatorDataSetTerminal(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetTerminal Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageOperatorDataSetTerminal Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _MaxByteLength, Tag);

        DecodedResult<BigInteger> result = _Codec.Decode(EncodingId, value).ToBigInteger()
            ?? throw new DataElementParsingException(EncodingId);

        return new DataStorageOperatorDataSetTerminal(result.Value);
    }

    #endregion
}