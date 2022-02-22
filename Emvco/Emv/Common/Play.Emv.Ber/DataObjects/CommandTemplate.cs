using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Emv.Ber.Codecs;

namespace Play.Emv.Ber.DataObjects;

/// <summary>
///     Identifies the data field of a command message
/// </summary>
public record CommandTemplate : DataElement<byte[]>, IEquatable<CommandTemplate>, IEqualityComparer<CommandTemplate>
{
    #region Static Metadata

    public static readonly PlayEncodingId PlayEncodingId = UnsignedBinaryCodec.Identifier;
    public static readonly Tag Tag = 0x83;

    #endregion

    #region Constructor

    public CommandTemplate(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetBerEncodingId() => PlayEncodingId;
    public int GetByteCount() => _Value.Length;
    public override Tag GetTag() => Tag;
    public byte[] GetValueAsByteArray() => _Value;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static CommandTemplate Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static CommandTemplate Decode(ReadOnlySpan<byte> value) => new(value.ToArray());

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

        if (_Value.Length != other._Value.Length)
            return false;

        for (int i = 0; i < _Value.Length; i++)
        {
            if (_Value[i] != other._Value[i])
                return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        const int hash = 45707;

        unchecked
        {
            int result = (int) (hash * GetTag());

            for (int i = 0; i < _Value.Length; i++)
                result += hash * _Value[i];

            return result;
        }
    }

    public int GetHashCode(CommandTemplate obj) => obj.GetHashCode();

    #endregion
}