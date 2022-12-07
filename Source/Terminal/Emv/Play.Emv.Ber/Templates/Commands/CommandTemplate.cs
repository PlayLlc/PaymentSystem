using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Templates;

/// <summary>
///     Identifies the data field of a command message
/// </summary>
public record CommandTemplate : DataElement<BigInteger>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x83;

    #endregion

    #region Constructor

    public CommandTemplate(BigInteger value) : base(value)
    { }

    public CommandTemplate(DataObjectListResult value) : base(new BigInteger(value.EncodeValue()))
    { }

    #endregion

    #region Serialization

    public static CommandTemplate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override CommandTemplate Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static CommandTemplate Decode(ReadOnlySpan<byte> value) => new(new BigInteger(value.ToArray()));

    public override byte[] EncodeValue() => _Value.ToByteArray(true);
    public override byte[] EncodeValue(int length) => PlayCodec.BinaryCodec.Encode(_Value.ToByteArray(true), length);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion
}