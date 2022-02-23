using Play.Ber.Codecs;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     The value to be appended to the ADF Name in the data field of the SELECT command, if the Extended Selection Support
///     flag is present and set to 1. Content is payment system proprietary.Note: The maximum length of Extended Selection
///     depends on the length of ADF Name in the same directory entry such that Length of Extended Selection + Length of
///     ADF Name <= 16.
/// </summary>
public record ExtendedSelection : DataElement<byte[]>, IEqualityComparer<ExtendedSelection>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly ExtendedSelection Default = new(Array.Empty<byte>());
    public static readonly Tag Tag = 0x9F29;

    #endregion

    #region Constructor

    public ExtendedSelection(ReadOnlySpan<byte> value) : base(value.ToArray())
    { }

    #endregion

    #region Instance Members

    public byte[] AsByteArray() => _Value.CopyValue();
    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;
    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetEncodingId(), _Value);
    public int GetValueByteCount() => _Value.Length;

    #endregion

    #region Serialization

    public static ExtendedSelection Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static ExtendedSelection Decode(ReadOnlySpan<byte> value) => new(value);

    #endregion

    #region Equality

    public bool Equals(ExtendedSelection? x, ExtendedSelection? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ExtendedSelection obj) => obj.GetHashCode();

    #endregion
}