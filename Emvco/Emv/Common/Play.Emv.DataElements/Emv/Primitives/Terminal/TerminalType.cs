using Play.Ber.Codecs;
using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Ber.Identifiers;
using Play.Ber.InternalFactories;
using Play.Emv.Ber.Codecs;
using Play.Emv.Ber.DataObjects;
using Play.Emv.DataElements.Exceptions;

namespace Play.Emv.DataElements;

/// <summary>
///     Indicates the environment of the terminal, its communications capability, and its operational control
/// </summary>
public partial record TerminalType : DataElement<byte>, IEqualityComparer<TerminalType>
{
    #region Static Metadata

    public static readonly BerEncodingId BerEncodingId = NumericDataElementCodec.Identifier;
    public static readonly Tag Tag = 0x9F35;
    private const byte _ByteLength = 2;

    #endregion

    #region Constructor

    /// <remarks>
    ///     This is purely to initialize a default instance according to Book C-2 Table 4.4
    /// </remarks>
    private TerminalType(byte value) : base(value)
    { }

    public TerminalType(Environment environment, CommunicationType communicationType, TerminalOperatorType terminalOperatorType) : base(
        (byte) (environment + communicationType + terminalOperatorType))
    { }

    #endregion

    #region Instance Members

    public TagLengthValue AsTagLengthValue(BerCodec codec) => new(GetTag(), EncodeValue(codec));
    public override BerEncodingId GetBerEncodingId() => BerEncodingId;

    /// <summary>
    ///     GetCommunicationType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public CommunicationType GetCommunicationType()
    {
        byte communicationTypeValue = (byte) ((_Value % 10) <= 3 ? _Value % 10 : (_Value % 10) - 3);

        if (!CommunicationType.TryGet(communicationTypeValue, out CommunicationType result))
            throw new InvalidOperationException($"There was an internal error trying to resolve {nameof(CommunicationType)}");

        return result;
    }

    public Environment GetEnvironment()
    {
        if ((_Value % 10) > 3)
            return Environment.Unattended;

        return Environment.Attended;
    }

    public override Tag GetTag() => Tag;

    /// <summary>
    ///     GetTerminalOperatorType
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public TerminalOperatorType GetTerminalOperatorType()
    {
        byte terminalOperatorTypeValue = (byte) ((_Value / 10) * 10);

        if (!TerminalOperatorType.TryGet(terminalOperatorTypeValue, out TerminalOperatorType result))
            throw new InvalidOperationException($"There was an internal error trying to resolve {nameof(CommunicationType)}");

        return result;
    }

    public override ushort GetValueByteCount(BerCodec codec) => codec.GetByteCount(GetBerEncodingId(), _Value);

    #endregion

    #region Serialization

    public static TerminalType Decode(ReadOnlyMemory<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(BerEncodingId, value.Span) as DecodedResult<byte>
            ?? throw new InvalidOperationException(
                $"The {nameof(TerminalType)} could not be initialized because the {nameof(BinaryDataElementCodec)} returned a null {nameof(DecodedResult<byte>)}");

        return new TerminalType(result.Value);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BerException"></exception>
    public static TerminalType Decode(ReadOnlySpan<byte> value)
    {
        Check.Primitive.ForExactLength(value, _ByteLength, Tag);

        DecodedResult<byte> result = _Codec.Decode(BerEncodingId, value).ToByteResult()
            ?? throw new DataElementNullException(BerEncodingId);

        return new TerminalType(result.Value);
    }

    public new byte[] EncodeValue() => EncodeValue(_ByteLength);

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
}