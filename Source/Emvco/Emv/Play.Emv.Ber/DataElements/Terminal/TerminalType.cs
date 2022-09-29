using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Codecs;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

/// <summary>
///     Indicates the environment of the terminal, its communications capability, and its operational control
/// </summary>
public partial record TerminalType : DataElement<byte>, IEqualityComparer<TerminalType>
{
    #region Static Metadata

    public static readonly PlayEncodingId EncodingId = NumericCodec.EncodingId;
    public static readonly Tag Tag = 0x9F35;
    private const byte _ByteLength = 1;

    #endregion

    #region Constructor

    /// <remarks>
    ///     This is purely to initialize a default instance according to Book C-2 Table 4.4
    /// </remarks>
    public TerminalType(byte value) : base(value)
    { }

    public TerminalType(EnvironmentType environmentType, CommunicationType communicationType, TerminalOperatorType terminalOperatorType) : base(
        (byte) (environmentType + communicationType + terminalOperatorType))
    { }

    #endregion

    #region Serialization

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalType Decode(ReadOnlyMemory<byte> value) => Decode(value.Span);

    public override TerminalType Decode(TagLengthValue value) => Decode(value.EncodeValue().AsSpan());

    /// <exception cref="DataElementParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public static TerminalType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        byte result = PlayCodec.NumericCodec.DecodeToByte(value);

        return new TerminalType(result);
    }

    public override byte[] EncodeValue() => PlayCodec.NumericCodec.Encode(_Value, _ByteLength);
    public override byte[] EncodeValue(int length) => PlayCodec.NumericCodec.Encode(_Value, length);

    #endregion

    #region Equality

    public bool Equals(TerminalType? x, TerminalType? y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(TerminalType obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static implicit operator byte(TerminalType value) => value._Value;

    #endregion

    #region Instance Members

    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public override PlayEncodingId GetEncodingId() => EncodingId;

    /// <summary>
    ///     GetCommunicationType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public CommunicationType GetCommunicationType()
    {
        byte communicationTypeValue = (byte) ((_Value % 10) <= 3 ? _Value % 10 : (_Value % 10) - 3);

        if (!CommunicationType.TryGet(communicationTypeValue, out CommunicationType result))
            throw new DataElementParsingException($"There was an internal error trying to resolve {nameof(CommunicationType)}");

        return result;
    }

    public EnvironmentType GetEnvironment()
    {
        if ((_Value % 10) > 3)
            return EnvironmentType.Unattended;

        return EnvironmentType.Attended;
    }

    public override Tag GetTag() => Tag;

    /// <summary>
    ///     GetTerminalOperatorType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public TerminalOperatorType GetTerminalOperatorType()
    {
        byte terminalOperatorTypeValue = (byte) ((_Value / 10) * 10);

        if (!TerminalOperatorType.TryGet(terminalOperatorTypeValue, out TerminalOperatorType result))
            throw new DataElementParsingException($"There was an internal error trying to resolve {nameof(CommunicationType)}");

        return result;
    }

    public bool IsOperatorType(TerminalOperatorType operatorType) => TerminalOperatorType.IsOperatorType(_Value, operatorType);
    public bool IsCommunicationType(CommunicationType communicationType) => CommunicationType.IsCommunicationType(_Value, communicationType);
    public bool IsEnvironmentType(EnvironmentType operatorType) => EnvironmentType.IsEnvironmentType(_Value, operatorType);
    public override ushort GetValueByteCount(BerCodec codec) => PlayCodec.NumericCodec.GetByteCount(_Value);
    public override ushort GetValueByteCount() => PlayCodec.NumericCodec.GetByteCount(_Value);

    #endregion
}