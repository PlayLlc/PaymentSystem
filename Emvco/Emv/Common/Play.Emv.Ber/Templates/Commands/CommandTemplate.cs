using System.Numerics;

using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Emv.Ber.DataElements;

namespace Play.Emv.Ber.Templates;

/// <summary>
///     Identifies the data field of a command message
/// </summary>
public record CommandTemplate : DataElement<BigInteger>, IEquatable<CommandTemplate>, IEqualityComparer<CommandTemplate>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x83;

    #endregion

    #region Constructor

    public CommandTemplate(BigInteger value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public int GetByteCount() => _Value.GetByteCount();
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CommandTemplate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);
    public override CommandTemplate Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerParsingException"></exception>
    public static CommandTemplate Decode(ReadOnlySpan<byte> value) => new(new BigInteger(value.ToArray()));

    #endregion

    #region Equality

    public bool Equals(CommandTemplate? x, CommandTemplate? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public virtual bool Equals(CommandTemplate? other)
    {
        if (other == null)
            return false;

        return _Value == other._Value;
    }

    public override int GetHashCode()
    {
        const int hash = 45707;

        unchecked
        {
            int result = (int) ((hash * GetTag()) + (hash * _Value));

            return result;
        }
    }

    public int GetHashCode(CommandTemplate obj) => obj.GetHashCode();

    #endregion
}