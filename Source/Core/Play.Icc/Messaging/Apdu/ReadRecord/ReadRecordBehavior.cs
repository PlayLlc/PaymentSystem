using Play.Icc.FileSystem.ElementaryFiles;

namespace Play.Icc.Messaging.Apdu.ReadRecord;

/// <summary>
///     Describes the behavior to be used in the <see cref="ReadRecordApduCommand" /> when a <see cref="RecordNumber" /> is
///     used
///     to identify the start position of the file pointer
/// </summary>
public readonly struct ReadRecordBehavior
{
    #region Static Metadata

    /// <summary>
    ///     Reads each record in a range starting from the last record in the Elementary File sequence to
    ///     the record identified by the <see cref="RecordNumber" />
    /// </summary>
    public static readonly ReadRecordBehavior FromEndToRecord = new(0b110);

    /// <summary>
    ///     Reads each record starting from the <see cref="RecordNumber" /> to the last record in the sequence
    ///     File seq
    /// </summary>
    public static readonly ReadRecordBehavior FromRecordToEnd = new(0b101);

    /// <summary>
    ///     Reads the record identified by the <see cref="RecordNumber" /> position
    /// </summary>
    public static readonly ReadRecordBehavior ReadRecord = new(0b100);

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    private ReadRecordBehavior(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Operator Overrides

    public static implicit operator byte(ReadRecordBehavior value) => value._Value;

    #endregion
}