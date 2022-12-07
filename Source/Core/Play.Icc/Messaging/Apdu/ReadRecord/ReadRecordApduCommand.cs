using System;

using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Icc.Messaging.Apdu.ReadRecord;

/// <summary>
/// </summary>
/// <remarks>
///     Book 3 Section 6.5.5
/// </remarks>
public class ReadRecordApduCommand : ApduCommand
{
    #region Constructor

    private ReadRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2) : base(@class, instruction, parameter1, parameter2)
    {
        throw new NotImplementedException("These constructors need to be private and personalized for this instance");
    }

    private ReadRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, uint le) : base(@class, instruction, parameter1, parameter2,
        le)
    {
        throw new NotImplementedException("These constructors need to be private and personalized for this instance");
    }

    private ReadRecordApduCommand(byte @class, byte instruction, byte parameter1, byte parameter2, ReadOnlySpan<byte> data) : base(@class, instruction,
        parameter1, parameter2, data)
    {
        throw new NotImplementedException("These constructors need to be private and personalized for this instance");
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Reads all the records from the elementary file indicated by the <see cref="ShortFileId" />. The
    ///     <see cref="ShortFileId" /> is only relevant within the currently selected Directory File
    /// </summary>
    /// <param name="shortFileId"></param>
    /// <returns></returns>
    public static ReadRecordApduCommand ReadAllRecords(ShortFileId shortFileId) =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.ReadRecord,
            0x00, // current record, which will be the first record
            (byte) ((shortFileId << 4) | ReadRecordBehavior.FromRecordToEnd), 0);

    /// <summary>
    ///     Reads the whole record from the currently selected Elementary File
    /// </summary>
    /// <param name="recordNumber"></param>
    /// <returns></returns>
    public static ReadRecordApduCommand ReadRecord(byte recordNumber) =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.ReadRecord, new RecordNumber(recordNumber),
            ReadRecordBehavior.ReadRecord, 0);

    /// <summary>
    ///     Selects an Elementary File matching the
    ///     <param name="shortFileIdentifier" />
    ///     and reads the record in the
    ///     ordinal position specified by
    ///     <param name="recordNumber" />
    /// </summary>
    /// <param name="shortFileIdentifier"></param>
    /// <param name="recordNumber"></param>
    /// <returns></returns>
    public static ReadRecordApduCommand ReadRecord(byte shortFileIdentifier, byte recordNumber) =>
        new(new Class(SecureMessaging.NotRecognized, LogicalChannel.BasicChannel), Instruction.ReadRecord, new RecordNumber(recordNumber),
            (byte) ((new ShortFileId(shortFileIdentifier) << 4) | ReadRecordBehavior.ReadRecord), 0);

    #endregion
}