using System.Runtime.CompilerServices;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Codecs;
using Play.Core.Extensions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     PunatcTrack2 stands for 'Position Of Unpredictable Number And Application Transaction Counter (Track2)'.
///     PUNATC(Track2) indicates to the Kernel the positions in the discretionary data field of Track 2 Data where the
///     Unpredictable Number digits and Application Transaction Counter digits have to be copied.
/// </summary>
public record PunatcTrack2 : DataElement<ushort>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = BinaryCodec.EncodingId;
    public static readonly Tag Tag = 0x9F66;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    public PunatcTrack2(ushort value) : base(value)
    { }

    #endregion

    public int GetSetBitCount() => _Value.GetSetBitCount();

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PunatcTrack2 Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override PunatcTrack2 Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="Codecs.Exceptions.CodecParsingException"></exception>
    public static PunatcTrack2 Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForMaximumLength(value, _ByteLength, Tag);

        ushort result = PlayCodec.BinaryCodec.DecodeToUInt16(value);

        return new PunatcTrack2(result);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

    #endregion

    #region Instance Members

    public override PlayEncodingId GetEncodingId() => EncodingId;
    public override Tag GetTag() => Tag;

    #endregion
}