using System.Collections.Immutable;

using Play.Core.Extensions;

namespace Play.Emv.DataElements;

public record ShortKernelId : IEqualityComparer<ShortKernelId>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, ShortKernelId> _ValueObjectMap;
    public static readonly ShortKernelId Kernel8;
    public static readonly ShortKernelId Kernel5;
    public static readonly ShortKernelId Kernel1;
    public static readonly ShortKernelId Kernel4;
    public static readonly ShortKernelId NotAvailable;
    public static readonly ShortKernelId Kernel2;
    public static readonly ShortKernelId Kernel7;
    public static readonly ShortKernelId Kernel6;
    public static readonly ShortKernelId Kernel3;

    #endregion

    #region Instance Values

    private readonly byte _Value;

    #endregion

    #region Constructor

    static ShortKernelId()
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

        NotAvailable = new ShortKernelId(notAvailable);
        Kernel1 = new ShortKernelId(firstKernel);
        Kernel2 = new ShortKernelId(secondKernel);
        Kernel3 = new ShortKernelId(thirdKernel);
        Kernel4 = new ShortKernelId(fourthKernel);
        Kernel5 = new ShortKernelId(fifthKernel);
        Kernel6 = new ShortKernelId(sixthKernel);
        Kernel7 = new ShortKernelId(seventhKernel);
        Kernel8 = new ShortKernelId(eighthKernel);
        _ValueObjectMap = new Dictionary<byte, ShortKernelId>
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

    private ShortKernelId(byte value)
    {
        _Value = value;
    }

    #endregion

    #region Instance Members

    public static ShortKernelId Get(byte value)
    {
        const byte bitMask = 0b11000000;

        if (!_ValueObjectMap.ContainsKey(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                                                  $"No {nameof(ShortKernelId)} could be retrieved because the argument provided does not match a definition value");
        }

        return _ValueObjectMap[value.GetMaskedValue(bitMask)];
    }

    #endregion

    #region Equality

    public bool Equals(ShortKernelId? x, ShortKernelId? y)
    {
        if (x == null)
            return y == null;

        if (y == null)
            return false;

        return x.Equals(y);
    }

    public bool Equals(byte other) => _Value == other;
    public int GetHashCode(ShortKernelId obj) => obj.GetHashCode();

    public override int GetHashCode()
    {
        const int hash = 543287;

        return hash + _Value.GetHashCode();
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(ShortKernelId left, byte right) => left._Value == right;
    public static bool operator ==(byte left, ShortKernelId right) => left == right._Value;
    public static explicit operator byte(ShortKernelId value) => value._Value;
    public static explicit operator ShortKernelId(byte value) => Get(value);
    public static implicit operator KernelId(ShortKernelId value) => new(value._Value);
    public static bool operator !=(ShortKernelId left, byte right) => !(left == right);
    public static bool operator !=(byte left, ShortKernelId right) => !(left == right);

    #endregion
}