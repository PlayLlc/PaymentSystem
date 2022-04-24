using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.DataElements;

public sealed record KernelType : EnumObject<byte>
{
    #region Static Metadata

    public static readonly KernelType Empty = new();
    private static readonly ImmutableSortedDictionary<byte, KernelType> _ValueObjectMap;
    public static readonly KernelType DomesticEmvCoKernel;
    public static readonly KernelType International;
    public static readonly KernelType ProprietaryDomesticEmvCoKernel;

    #endregion

    #region Constructor

    public KernelType() : base()
    { }

    static KernelType()
    {
        const byte international = 0;
        const byte domesticEmvCoKernel = 2;
        const byte proprietaryDomesticEmvCoKernel = 3;

        International = new KernelType(international);
        DomesticEmvCoKernel = new KernelType(domesticEmvCoKernel);
        ProprietaryDomesticEmvCoKernel = new KernelType(proprietaryDomesticEmvCoKernel);
        _ValueObjectMap = new Dictionary<byte, KernelType>
        {
            {international, International}, {domesticEmvCoKernel, DomesticEmvCoKernel}, {proprietaryDomesticEmvCoKernel, ProprietaryDomesticEmvCoKernel}
        }.ToImmutableSortedDictionary();
    }

    private KernelType(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override KernelType[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out KernelType? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    public static bool TryGet(byte value, out KernelType? result) => _ValueObjectMap.TryGetValue(value, out result);

    #endregion

    #region Equality

    public bool Equals(KernelType? other) => !(other is null) && (_Value == other._Value);

    public bool Equals(KernelType x, KernelType y)
    {
        if (x is null)
            return y is null;

        if (y is null)
            return false;

        return x.Equals(y);
    }

    public override int GetHashCode() => 470621 * _Value.GetHashCode();
    public int GetHashCode(KernelType obj) => obj.GetHashCode();
    public int CompareTo(KernelType other) => _Value.CompareTo(other._Value);

    #endregion

    #region Operator Overrides

    public static explicit operator byte(KernelType registeredApplicationProviderIndicators) => registeredApplicationProviderIndicators._Value;

    #endregion
}