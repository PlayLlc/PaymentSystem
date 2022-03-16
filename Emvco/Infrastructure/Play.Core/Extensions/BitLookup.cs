using Play.Core.Exceptions;

namespace Play.Core.Extensions;

public static class BitLookup
{
    #region Static Metadata

    private const byte _Eight = (byte) Bits.Eight;
    private const byte _Seven = (byte) Bits.Seven;
    private const byte _Six = (byte) Bits.Six;
    private const byte _Five = (byte) Bits.Five;
    private const byte _Four = (byte) Bits.Four;
    private const byte _Three = (byte) Bits.Three;
    private const byte _Two = (byte) Bits.Two;
    private const byte _One = (byte) Bits.One;

    #endregion

    #region Instance Members

    public static byte GetBit(Bits bit)
    {
        return bit switch
        {
            Bits.Eight => _Eight,
            Bits.Seven => _Seven,
            Bits.Six => _Six,
            Bits.Five => _Five,
            Bits.Four => _Four,
            Bits.Three => _Three,
            Bits.Two => _Two,
            Bits.One => _One
        };
    }

    #endregion
}