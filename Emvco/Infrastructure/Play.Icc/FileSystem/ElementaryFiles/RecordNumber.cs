using System;

using Play.Icc.Exceptions;

namespace Play.Icc.FileSystem.ElementaryFiles;

public readonly struct RecordNumber
{
    #region Static Metadata

    private const byte _MinValue = 1;
    private const byte _MaxValue = 254;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    public RecordNumber(byte value)
    {
        if ((value < _MinValue) || (value > _MaxValue))
        {
            throw new IccProtocolException(new ArgumentOutOfRangeException(nameof(value),
                $"The argument {nameof(value)} value {value} was out of range. {nameof(RecordNumber)} must be initialized with a value between {_MinValue} and {_MaxValue}"));
        }

        _Value = value;
    }

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is RecordNumber recordNumber && Equals(recordNumber);
    public bool Equals(RecordNumber other) => _Value == other._Value;
    public bool Equals(RecordNumber x, RecordNumber y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 68791;

        return hash * _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(RecordNumber left, RecordNumber right) => left._Value == right._Value;
    public static bool operator ==(RecordNumber left, byte right) => left._Value == right;
    public static bool operator ==(byte left, RecordNumber right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information 
    public static implicit operator byte(RecordNumber value) => value._Value;
    public static bool operator !=(RecordNumber left, RecordNumber right) => !(left == right);
    public static bool operator !=(RecordNumber left, byte right) => !(left == right);
    public static bool operator !=(byte left, RecordNumber right) => !(left == right);

    #endregion
}