using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Icc;

internal record Instruction : EnumObject<byte>, IComparable<Instruction>
{
    #region Static Metadata

    private static readonly ImmutableSortedDictionary<byte, Instruction> _ValueObjectMap;

    /// <value>Hexadecimal: 0x1E</value>
    public static readonly Instruction ApplicationBlock;

    /// <value>Hexadecimal: 0x18</value>
    public static readonly Instruction ApplicationUnblock;

    /// <value>Hexadecimal: 0x16</value>
    public static readonly Instruction CardBlock;

    /// <value>Hexadecimal: 0x2A</value>
    public static readonly Instruction ComputeCryptographicChecksum;

    /// <value>Hexadecimal: 0xA8</value>
    public static readonly Instruction GetProcessingOptions;

    /// <value>Hexadecimal: 0xEA</value>
    public static readonly Instruction ExchangeRelayResistanceData;

    /// <value>Hexadecimal: 0xD0</value>
    public static readonly Instruction RecoverApplicationCryptogram;

    #endregion

    #region Constructor

    static Instruction()
    {
        const byte getProcessingOptions = 0xA8;
        const byte applicationBlock = 0x1E;
        const byte applicationUnblock = 0x18;
        const byte cardBlock = 0x16;
        const byte computeCryptographicChecksum = 0x2A;

        GetProcessingOptions = new Instruction(getProcessingOptions);
        ApplicationBlock = new Instruction(applicationBlock);
        ApplicationUnblock = new Instruction(applicationUnblock);
        CardBlock = new Instruction(cardBlock);
        ComputeCryptographicChecksum = new Instruction(computeCryptographicChecksum);
        ExchangeRelayResistanceData = new Instruction(0xEA);
        RecoverApplicationCryptogram = new Instruction(0xD0);
        _ValueObjectMap = new Dictionary<byte, Instruction>
        {
            {getProcessingOptions, GetProcessingOptions},
            {applicationBlock, ApplicationBlock},
            {applicationUnblock, ApplicationUnblock},
            {cardBlock, CardBlock},
            {computeCryptographicChecksum, ComputeCryptographicChecksum},
            {ExchangeRelayResistanceData, ExchangeRelayResistanceData},
            {RecoverApplicationCryptogram, RecoverApplicationCryptogram}
        }.ToImmutableSortedDictionary();
    }

    private Instruction(byte value) : base(value)
    { }

    #endregion

    #region Instance Members

    public override Instruction[] GetAll() => _ValueObjectMap.Values.ToArray();

    public override bool TryGet(byte value, out EnumObject<byte>? result)
    {
        if (_ValueObjectMap.TryGetValue(value, out Instruction? enumResult))
        {
            result = enumResult;

            return true;
        }

        result = null;

        return false;
    }

    #endregion

    #region Equality

    public bool Equals(Instruction x, Instruction y) => x.Equals(y);

    public override int GetHashCode()
    {
        const int hash = 658379;

        return hash + _Value.GetHashCode();
    }

    public int CompareTo(Instruction? other)
    {
        if (other is null)
            return 1;

        return _Value.CompareTo(other);
    }

    #endregion

    #region Operator Overrides

    public static bool operator ==(Instruction left, byte right) => left._Value == right;
    public static bool operator ==(byte left, Instruction right) => left == right._Value;

    // logical channel values are from 0 to 3 so casting to sbyte will not truncate any meaningful information
    public static explicit operator sbyte(Instruction value) => (sbyte) value._Value;
    public static explicit operator short(Instruction value) => value._Value;
    public static explicit operator ushort(Instruction value) => value._Value;
    public static explicit operator int(Instruction value) => value._Value;
    public static explicit operator uint(Instruction value) => value._Value;
    public static explicit operator long(Instruction value) => value._Value;
    public static explicit operator ulong(Instruction value) => value._Value;
    public static implicit operator byte(Instruction value) => value._Value;
    public static bool operator !=(Instruction left, byte right) => !(left == right);
    public static bool operator !=(byte left, Instruction right) => !(left == right);

    #endregion
}