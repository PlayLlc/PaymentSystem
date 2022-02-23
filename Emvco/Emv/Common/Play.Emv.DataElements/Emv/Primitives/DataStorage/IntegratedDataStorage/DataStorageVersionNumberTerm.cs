using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Codecs.Strings;
using Play.Emv.Ber.DataObjects;

namespace Play.Emv.DataElements.Emv;

/// <summary>
///     Integrated data storage support by the Kernel depends on the  presence of this data object. If it is absent, or is
///     present with a  length of zero, integrated data storage is not supported. Its value is '02' for this version of
///     data storage functionality. This variable length data item has an initial byte that defines  the maximum version
///     number supported by the Terminal and a  variable number of subsequent bytes that define how the  Terminal supports
///     earlier versions of the specification. As this  is the first version, no legacy support is described and no
///     additional bytes are present.
/// </summary>
public record DataStorageVersionNumberTerm : DataElement<byte[]>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0xDF810D;

    #endregion

    #region Constructor

    public DataStorageVersionNumberTerm(byte[] value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion

    #region Serialization

    public static DataStorageVersionNumberTerm Decode(ReadOnlySpan<byte> value) => new(value.ToArray());

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static DataStorageVersionNumberTerm Decode(ReadOnlyMemory<byte> value) => new(value.ToArray());

    #endregion
}