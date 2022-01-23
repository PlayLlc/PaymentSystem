using System;

namespace Play.Icc.Messaging.Activation;

public class UniqueIdentifierTypeA
{
    #region Constructor

    public UniqueIdentifierTypeA(ReadOnlySpan<byte> value)
    {
        const byte validLength4 = 4;
        const byte validLength7 = 7;
        const byte validLength10 = 10;

        if ((value.Length != 4) && (value.Length != 7) && (value.Length != 10))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"The argument {nameof(value)} must be {validLength4}, {validLength7}, or {validLength10} bytes to initialize a {nameof(UniqueIdentifierTypeA)}");
        }
    }

    #endregion
}