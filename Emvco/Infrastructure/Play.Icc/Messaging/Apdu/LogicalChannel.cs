using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core.Extensions;
using Play.Icc.Exceptions;

namespace Play.Icc.Messaging.Apdu;

/// ///
/// <summary>
///     A logical channel works as a logical link to a DF. Commands referring to a certain logical channel carry the
///     respective logical channel number in the
///     CLA byte. Logical channels are numbered from 0 to 3. Notes:
///     1. More than one logical channel may be opened to the same DF, if not excluded (see file accessibility in 5.1.5)
///     2. More than one logical channel may select the same EF if not excluded(see file accessibility in 5.1.5)
///     3. A SELECT FILE command on any logical channel will open a current DF and possibly a current EF.
///     Therefore, there is one current DF and possibly one current EF per logical channel as a result
///     of the behavior of the SELECT FILE command file accessing commands using a short EF identifier.
/// </summary>
/// <remarks>ISO-IEC 7816 section 5.5.1</remarks>
public readonly struct LogicalChannel
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, LogicalChannel> _ValueObjectMap;

    /// <summary>
    ///     The basic logical channel is permanently available. When numbered, its number is 0. When the class
    ///     byte is coded according to table 8 and 9, the bits b1 and b2 code the logical channel number.
    /// </summary>
    public static readonly LogicalChannel BasicChannel;

    public static readonly LogicalChannel ChannelOne;
    public static readonly LogicalChannel ChannelThree;
    public static readonly LogicalChannel ChannelTwo;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static LogicalChannel()
    {
        const byte basicChannel = 0;
        const byte channelOne = 1;
        const byte channelTwo = 2;
        const byte channelThree = 3;

        BasicChannel = new LogicalChannel(basicChannel);
        ChannelOne = new LogicalChannel(channelOne);
        ChannelTwo = new LogicalChannel(channelTwo);
        ChannelThree = new LogicalChannel(channelThree);

        _ValueObjectMap = new Dictionary<byte, LogicalChannel>
        {
            {basicChannel, BasicChannel}, {channelOne, ChannelOne}, {channelTwo, ChannelTwo}, {channelThree, ChannelThree}
        }.ToImmutableSortedDictionary();
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="IccProtocolException"></exception>
    private LogicalChannel(byte value)
    {
        if (value > 3)
            throw new IccProtocolException(nameof(value));

        _Value = value;
    }

    #endregion

    #region Instance Members

    public static LogicalChannel Get(byte value)
    {
        const byte logicalChannelMask = 0xFC;

        return _ValueObjectMap[value.GetMaskedValue(logicalChannelMask)];
    }

    public static bool IsValid(byte value) => _ValueObjectMap.ContainsKey(value);

    #endregion

    #region Equality

    public override bool Equals(object? obj) => obj is LogicalChannel logicalChannel && Equals(logicalChannel);
    public bool Equals(LogicalChannel other) => _Value == other._Value;
    public bool Equals(LogicalChannel x, LogicalChannel y) => x.Equals(y);
    public bool Equals(byte other) => _Value == other;

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(LogicalChannel left, LogicalChannel right) => left._Value == right._Value;
    public static bool operator ==(LogicalChannel left, byte right) => left._Value == right;
    public static bool operator ==(byte left, LogicalChannel right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(LogicalChannel value) => (sbyte) value._Value;
    public static explicit operator short(LogicalChannel value) => value._Value;
    public static explicit operator ushort(LogicalChannel value) => value._Value;
    public static explicit operator int(LogicalChannel value) => value._Value;
    public static explicit operator uint(LogicalChannel value) => value._Value;
    public static explicit operator long(LogicalChannel value) => value._Value;
    public static explicit operator ulong(LogicalChannel value) => value._Value;
    public static implicit operator byte(LogicalChannel value) => value._Value;
    public static bool operator !=(LogicalChannel left, LogicalChannel right) => !(left == right);
    public static bool operator !=(LogicalChannel left, byte right) => !(left == right);
    public static bool operator !=(byte left, LogicalChannel right) => !(left == right);

    #endregion
}