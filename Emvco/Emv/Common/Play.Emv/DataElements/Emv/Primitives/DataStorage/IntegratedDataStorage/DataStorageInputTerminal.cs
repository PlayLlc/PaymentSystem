using System;
using System.Collections.Generic;
using System.Linq;
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
///     Contains Terminal provided data if permanent data storage in the Card was applicable (DS Slot Management
///     Control[8]=1b), remains applicable or becomes applicable (DS ODS Info[8]=1b). DS Input (Term) is used by the Kernel
///     as input to calculate DS Digest H.
/// </summary>
public record DataStorageInputTerminal : DataElement<byte>
{
    #region Static Metadata

    public static readonly Tag Tag = 0xDF8109;
    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    private const byte _ByteLength = 8;

    #endregion

    #region Constructor

    public DataStorageInputTerminal(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageInputTerminal Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static DataStorageInputTerminal Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<ulong> result = _Codec.Decode(EncodingId, value).ToUInt64Result()
            ?? throw new DataElementParsingException(EncodingId);

        return new DataStorageInputTerminal(value[0]);
    }

    #endregion
}