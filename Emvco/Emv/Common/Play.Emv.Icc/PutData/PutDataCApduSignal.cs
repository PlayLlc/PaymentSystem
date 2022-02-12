using Play.Ber.Identifiers;
using Play.Core;
using Play.Icc.Messaging.Apdu;
using Play.Icc.Messaging.Apdu.PutData;

namespace Play.Emv.Icc.PutData;

/// <summary>
///     Stores primitive data that is not encapsulated in an Elementary File record
/// </summary>
public class PutDataCApduSignal : CApduSignal
{
    #region Constructor

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
     parameter2)
    { }

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction,
     parameter1, parameter2, le)
    { }

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) :
        base(@class, instruction, parameter1, parameter2, data)
    { }

    private PutDataCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    /// <param name="dataObject">
    ///     The <see cref="Tag" /> must be less than or equal to a <see cref="ushort" /> value
    /// </param>
    /// <returns></returns>
    public static PutDataCApduSignal Create(DataObject dataObject)
    {
        PutDataApduCommand? command = PutDataApduCommand.Create(ProprietaryMessageIdentifier._8x, (ushort) dataObject.GetTag());

        return new PutDataCApduSignal(command.GetClass(), command.GetInstruction(), command.GetParameter1(), command.GetParameter2(),
                                      command.GetData());
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