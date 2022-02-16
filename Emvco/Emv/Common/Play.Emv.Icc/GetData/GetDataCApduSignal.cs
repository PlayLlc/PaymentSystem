using Play.Ber.Identifiers;
using Play.Core;
using Play.Icc.Messaging.Apdu;
using Play.Icc.Messaging.Apdu.GetData;

namespace Play.Emv.Icc.GetData;

/// <summary>
///     Gets a TLV encoded object that is not encapsulated as an Elementary File Record
/// </summary>
public class GetDataCApduSignal : CApduSignal
{
    #region Constructor

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    private GetDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) : base(
        @class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <param name="tag">
    ///     The <see cref="Tag" /> must be less than or equal to a <see cref="ushort" /> value
    /// </param>
    /// <returns></returns>
    public static GetDataCApduSignal Create(DataObject dataObject)
    {
        GetDataApduCommand cApdu = GetDataApduCommand.Create(ProprietaryMessageIdentifier._8x, (ushort) dataObject.GetTag());

        return new GetDataCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetData(), cApdu.GetLe());
    }

    #endregion

    /// <summary>
    ///     Supported values for the Get Data command
    /// </summary>
    public sealed record DataObject : EnumObject<Tag>
    {
        #region Static Metadata

        public static readonly DataObject UnprotectedDataEnvelope1 = new(0x9F75);
        public static readonly DataObject UnprotectedDataEnvelope2 = new(0x9F76);
        public static readonly DataObject UnprotectedDataEnvelope3 = new(0x9F77);
        public static readonly DataObject UnprotectedDataEnvelope4 = new(0x9F78);
        public static readonly DataObject UnprotectedDataEnvelope5 = new(0x9F79);
        public static readonly DataObject OfflineAccumulatorBalance = new(0x9F50);
        public static readonly DataObject ProtectedDataEnvelope1 = new(0x9F70);
        public static readonly DataObject ProtectedDataEnvelope2 = new(0x9F71);
        public static readonly DataObject ProtectedDataEnvelope3 = new(0x9F72);
        public static readonly DataObject ProtectedDataEnvelope4 = new(0x9F73);
        public static readonly DataObject ProtectedDataEnvelope5 = new(0x9F74);

        #endregion

        #region Constructor

        private DataObject(Tag value) : base(value)
        { }

        #endregion

        #region Instance Members

        public Tag GetTag() => _Value;

        #endregion
    }
}