using System.Collections.Immutable;

using Play.Core;
using Play.Core.Extensions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.Enums;

public sealed record ShortKernelIdTypes : EnumObject<byte>, IEqualityComparer<byte> { public override ShortKernelIdTypes[] GetAll() => _ValueObjectMap.Values.ToArray(); public override bool TryGet(byte value, out EnumObject<byte>? result) { if (_ValueObjectMap.TryGetValue(value, out ShortKernelIdTypes? enumResult)) { result = enumResult; return true; } result = null; return false; }
 #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, ShortKernelIdTypes> _ValueObjectMap;
    public static readonly ShortKernelIdTypes Kernel8;
    public static readonly ShortKernelIdTypes Kernel5;
    public static readonly ShortKernelIdTypes Kernel1;
    public static readonly ShortKernelIdTypes Kernel4;
    public static readonly ShortKernelIdTypes NotAvailable;
    public static readonly ShortKernelIdTypes Kernel2;
    public static readonly ShortKernelIdTypes Kernel7;
    public static readonly ShortKernelIdTypes Kernel6;
    public static readonly ShortKernelIdTypes Kernel3;

    #endregion

    #region Constructor

    static ShortKernelIdTypes()
    {
        const byte notAvailable = 0;
        const byte firstKernel = 1;
        const byte secondKernel = 2;
        const byte thirdKernel = 3;
        const byte fourthKernel = 4;
        const byte fifthKernel = 5;
        const byte sixthKernel = 6;
        const byte seventhKernel = 7;
        const byte eighthKernel = 8;

        NotAvailable = new ShortKernelIdTypes(notAvailable);
        Kernel1 = new ShortKernelIdTypes(firstKernel);
        Kernel2 = new ShortKernelIdTypes(secondKernel);
        Kernel3 = new ShortKernelIdTypes(thirdKernel);
        Kernel4 = new ShortKernelIdTypes(fourthKernel);
        Kernel5 = new ShortKernelIdTypes(fifthKernel);
        Kernel6 = new ShortKernelIdTypes(sixthKernel);
        Kernel7 = new ShortKernelIdTypes(seventhKernel);
        Kernel8 = new ShortKernelIdTypes(eighthKernel);
        _ValueObjectMap = new Dictionary<byte, ShortKernelIdTypes>
        {
            {firstKernel, Kernel1},
            {secondKernel, Kernel2},
            {thirdKernel, Kernel3},
            {fourthKernel, Kernel4},
            {fifthKernel, Kernel5},
            {sixthKernel, Kernel6},
            {seventhKernel, Kernel7},
            {eighthKernel, Kernel8}
        }.ToImmutableSortedDictionary();
    }

    private ShortKernelIdTypes(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public static ShortKernelIdTypes[] GetAll() => _ValueObjectMap.Values.ToArray();

    /// <summary>
    ///     Get
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="DataElementParsingException"></exception>
    public static ShortKernelIdTypes Get(byte value)
    {
        const byte bitMask = 0b11000000;

        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new DataElementParsingException(new ArgumentOutOfRangeException(nameof(value),
                $"No {nameof(ShortKernelIdTypes)} could be retrieved because the argument provided does not match a definition value"));
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(ShortKernelIdTypes? x, ShortKernelIdTypes? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(ShortKernelIdTypes obj) => obj.GetHashCode();

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortKernelIdTypes left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ShortKernelIdTypes right) => left == right._Value;
    public static explicit operator byte(ShortKernelIdTypes value) => value._Value;
    public static explicit operator ShortKernelIdTypes(byte value) => Get(value);
    public static implicit operator KernelId(ShortKernelIdTypes value) => new(value._Value);
    public static bool operator !=(ShortKernelIdTypes left, byte right) => !(left == right);
    public static bool operator !=(byte left, ShortKernelIdTypes right) => !(left == right);

    #endregion
}