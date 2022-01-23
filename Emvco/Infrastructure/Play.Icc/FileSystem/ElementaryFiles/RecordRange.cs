using Play.Ber.Identifiers;

namespace Play.Icc.FileSystem.ElementaryFiles;

/// <summary>
///     Identifies an Elementary File of record structure and a sequential range of records by identifying the ordinal
///     position
///     of the first and last record in this range
/// </summary>
public readonly struct RecordRange
{
    #region Instance Values

    /// <summary>
    ///     The position of the first record in a range of records for an Elementary File
    /// </summary>
    private readonly byte _FirstRecordOrdinal;

    /// <summary>
    ///     The position of the last record in a range of records for an Elementary File
    /// </summary>
    private readonly byte _LastRecordOrdinal;

    /// <summary>
    ///     The number of records in this range of records
    /// </summary>
    private readonly byte _Length;

    /// <summary>
    ///     The Short File Identifier of an Elementary File relevant to an Application
    /// </summary>
    private readonly byte _ShortFileIdentifier;

    #endregion

    #region Constructor

    public RecordRange(uint value)
    {
        _ShortFileIdentifier = (byte) (value >> 24);
        _FirstRecordOrdinal = (byte) (value >> 16);
        _LastRecordOrdinal = (byte) (value >> 8);
        _Length = (byte) value;
    }

    public RecordRange(byte shortFileIdentifier, byte firstRecordOrdinal, byte lastRecordOrdinal, byte length)
    {
        _ShortFileIdentifier = shortFileIdentifier;
        _FirstRecordOrdinal = firstRecordOrdinal;
        _LastRecordOrdinal = lastRecordOrdinal;
        _Length = length;
    }

    #endregion

    #region Instance Members

    public byte GetFirstRecordOrdinal() => _FirstRecordOrdinal;
    public byte GetLastRecordOrdinal() => _LastRecordOrdinal;
    public byte GetRangeLength() => _Length;

    public RecordNumber[] GetRecords()
    {
        RecordNumber[] result = new RecordNumber[_Length];
        for (byte i = 0, j = _FirstRecordOrdinal; i < _Length; i++, j++)
            result[i] = new RecordNumber(j);

        return result;
    }

    public ShortFileId GetShortFileIdentifier() => new(_ShortFileIdentifier);
    public bool IsElementaryFile(Tag tag) => _ShortFileIdentifier == tag;

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is RecordRange recordRange && Equals(recordRange);

    public bool Equals(RecordRange other) =>
        (_ShortFileIdentifier == other._ShortFileIdentifier)
        && (_FirstRecordOrdinal == other._FirstRecordOrdinal)
        && (_LastRecordOrdinal == other._LastRecordOrdinal)
        && (_Length == other._Length);

    public bool Equals(RecordRange x, RecordRange y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 83003;

        unchecked
        {
            int result = 0;
            result += hash * _ShortFileIdentifier.GetHashCode();
            result += hash * _FirstRecordOrdinal.GetHashCode();
            result += hash * _LastRecordOrdinal.GetHashCode();
            result += hash * _Length.GetHashCode();

            return result;
        }
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(RecordRange left, RecordRange right) => left.Equals(right);
    public static bool operator !=(RecordRange left, RecordRange right) => !left.Equals(right);

    #endregion
}