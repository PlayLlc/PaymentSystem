using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Play.Core.Extensions;

[Flags]
public enum Bits : byte
{
    One = ByteSpec.One,
    Two = ByteSpec.Two,
    Three = ByteSpec.Three,
    Four = ByteSpec.Four,
    Five = ByteSpec.Five,
    Six = ByteSpec.Six,
    Seven = ByteSpec.Seven,
    Eight = ByteSpec.Eight
}

#region Signed Integers

#endregion