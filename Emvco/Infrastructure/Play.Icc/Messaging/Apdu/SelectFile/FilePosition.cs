using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Icc.Messaging.Apdu.SelectFile;

public readonly struct FilePosition
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, FilePosition> _ValueObjectMap;

    /// <value>0x00</value>
    public static readonly FilePosition FirstOrOnly;

    /// <value>0x01</value>
    public static readonly FilePosition Last;

    /// <value>0x02</value>
    public static readonly FilePosition Next;

    /// <value>0x03</value>
    public static readonly FilePosition Previous;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static FilePosition()
    {
        const byte firstOrOnly = 0x00;
        const byte last = 0x01;
        const byte next = 0x02;
        const byte previous = 0x03;

        FirstOrOnly = new FilePosition(firstOrOnly);
        Last = new FilePosition(last);
        Next = new FilePosition(next);
        Previous = new FilePosition(previous);

        _ValueObjectMap = new Dictionary<byte, FilePosition> {{firstOrOnly, FirstOrOnly}, {last, Last}, {next, Next}, {previous, Previous}}
            .ToImmutableSortedDictionary();
    }

    private FilePosition(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Equality

    //public static FilePosition Get(byte value)
    //{
    //    const byte secureMessagingMask = (byte)(BitCount.Eight | BitCount.Seven | BitCount.Six | BitCount.Five);
    //    return _ValueObjectMap[value.GetMaskedValue(secureMessagingMask)];
    //}
    public override bool Equals(object? obj) => obj is FilePosition secureMessaging && Equals(secureMessaging);
    public bool Equals(FilePosition other) => _Value == other._Value;
    public bool Equals(FilePosition x, FilePosition y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 10544431;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(FilePosition left, FilePosition right) => left._Value == right._Value;
    public static bool operator ==(FilePosition left, byte right) => left._Value == right;
    public static bool operator ==(byte left, FilePosition right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(FilePosition value) => (sbyte) value._Value;
    public static explicit operator short(FilePosition value) => value._Value;
    public static explicit operator ushort(FilePosition value) => value._Value;
    public static explicit operator int(FilePosition value) => value._Value;
    public static explicit operator uint(FilePosition value) => value._Value;
    public static explicit operator long(FilePosition value) => value._Value;
    public static explicit operator ulong(FilePosition value) => value._Value;
    public static implicit operator byte(FilePosition value) => value._Value;
    public static bool operator !=(FilePosition left, FilePosition right) => !(left == right);
    public static bool operator !=(FilePosition left, byte right) => !(left == right);
    public static bool operator !=(byte left, FilePosition right) => !(left == right);

    #endregion
}