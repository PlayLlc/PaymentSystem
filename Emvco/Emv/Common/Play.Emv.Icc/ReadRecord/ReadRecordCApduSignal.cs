using Play.Icc.FileSystem.ElementaryFiles;
using Play.Icc.Messaging.Apdu.ReadRecord;

namespace Play.Emv.Icc.ReadRecord;

public class ReadRecordCApduSignal : CApduSignal
{
    #region Constructor

    protected ReadRecordCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1,
        parameter2)
    { }

    protected ReadRecordCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, uint? le) : base(@class, instruction,
        parameter1, parameter2, le)
    { }

    protected ReadRecordCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class,
        instruction, parameter1, parameter2, data)
    { }

    protected ReadRecordCApduSignal(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data, uint? le) :
        base(@class, instruction, parameter1, parameter2, data, le)
    { }

    #endregion

    #region Instance Members

    public static ReadRecordCApduSignal ReadAllRecords(ShortFileId shortFileIdentifier)
    {
        ReadRecordApduCommand cApdu = ReadRecordApduCommand.ReadAllRecords(shortFileIdentifier);

        return new ReadRecordCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetLe());
    }

    /// <summary>
    ///     Selects an Elementary File matching the
    ///     <param name="shortFileIdentifier" />
    ///     and reads the record in the
    ///     ordinal position specified by
    ///     <param name="recordNumber" />
    /// </summary>
    public static ReadRecordCApduSignal ReadRecord(byte shortFileIdentifier, byte recordNumber)
    {
        ReadRecordApduCommand cApdu = ReadRecordApduCommand.ReadRecord(shortFileIdentifier, recordNumber);

        return new ReadRecordCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetLe());
    }

    /// <summary>
    ///     Reads the whole recordNumber from the currently selected Elementary File
    /// </summary>
    public static ReadRecordCApduSignal ReadRecord(byte recordNumber)
    {
        ReadRecordApduCommand cApdu = ReadRecordApduCommand.ReadRecord(recordNumber);

        return new ReadRecordCApduSignal(cApdu.GetClass(), cApdu.GetInstruction(), cApdu.GetParameter1(), cApdu.GetParameter2(),
            cApdu.GetData(), cApdu.GetLe());
    }

    #endregion
}