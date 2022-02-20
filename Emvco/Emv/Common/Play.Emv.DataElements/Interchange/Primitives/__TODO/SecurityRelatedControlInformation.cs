using Play.Codecs;
using Play.Core;
using Play.Emv.Interchange;
using Play.Emv.Interchange.Codecs;
using Play.Interchange.Codecs;
using Play.Interchange.DataFields;

namespace Play.Emv.Acquirers.Elavon.DataFields;

public record SecurityRelatedControlInformation : EmvDataField<char[]>
{
    #region Static Metadata

    public static readonly DataFieldId DataFieldId = 53;
    public static readonly InterchangeEncodingId EncodingId = AlphaNumericInterchangeDataFieldCodec.Identifier;

    #endregion

    #region Constructor

    public SecurityRelatedControlInformation(PinKeyTypeIdentifier pinPinKeyType, PinKeyIdentifier pinKeyIdentifier) : base(
        Create(pinPinKeyType, pinKeyIdentifier))
    { }

    #endregion

    #region Instance Members

    private static char[] Create(PinKeyTypeIdentifier pinKeyTypeIdentifier, PinKeyIdentifier pinKeyIdentifier)
    {
        Span<char> buffer = stackalloc char[16];
        buffer.Fill('0');
        int offset = 0;

        byte[] value = {pinKeyTypeIdentifier, pinKeyIdentifier};

        PlayEncoding.Numeric.GetChars(value, buffer, ref offset);

        return buffer.ToArray();
    }

    public override DataFieldId GetDataFieldId() => DataFieldId;
    public override InterchangeEncodingId GetEncodingId() => EncodingId;

    #endregion

    public record PinKeyTypeIdentifier : EnumObject<byte>
    {
        #region Static Metadata

        public static PinKeyTypeIdentifier TerminalPinPinKey = new(01);
        public static PinKeyTypeIdentifier ZonePinPinKey = new(02);

        #endregion

        #region Constructor

        private PinKeyTypeIdentifier(byte value) : base(value)
        { }

        #endregion
    }

    public record PinKeyIdentifier : EnumObject<byte>
    {
        #region Static Metadata

        public static PinKeyIdentifier _3 = new(3);
        public static PinKeyIdentifier _4 = new(4);
        public static PinKeyIdentifier _5 = new(5);
        public static PinKeyIdentifier _6 = new(6);
        public static PinKeyIdentifier _7 = new(7);
        public static PinKeyIdentifier _8 = new(8);
        public static PinKeyIdentifier _9 = new(9);
        public static PinKeyIdentifier _10 = new(10);
        public static PinKeyIdentifier _11 = new(11);
        public static PinKeyIdentifier _12 = new(12);
        public static PinKeyIdentifier _13 = new(13);
        public static PinKeyIdentifier _14 = new(14);
        public static PinKeyIdentifier _15 = new(15);
        public static PinKeyIdentifier _16 = new(16);
        public static PinKeyIdentifier _17 = new(17);
        public static PinKeyIdentifier _18 = new(18);
        public static PinKeyIdentifier _19 = new(19);
        public static PinKeyIdentifier _20 = new(20);
        public static PinKeyIdentifier _21 = new(21);
        public static PinKeyIdentifier _22 = new(22);
        public static PinKeyIdentifier _23 = new(23);
        public static PinKeyIdentifier _24 = new(24);
        public static PinKeyIdentifier _25 = new(25);
        public static PinKeyIdentifier _26 = new(26);
        public static PinKeyIdentifier _27 = new(27);
        public static PinKeyIdentifier _28 = new(28);
        public static PinKeyIdentifier _29 = new(29);
        public static PinKeyIdentifier _30 = new(30);
        public static PinKeyIdentifier _31 = new(31);
        public static PinKeyIdentifier _32 = new(32);
        public static PinKeyIdentifier _33 = new(33);

        #endregion

        #region Constructor

        private PinKeyIdentifier(byte value) : base(value)
        { }

        #endregion
    }
}