using System.Collections.Immutable;

using Play.Core;
using Play.Core.Exceptions;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public partial record TerminalType
{
    public record TerminalOperatorType : EnumObject<byte>
    {
        #region Static Metadata

        public static readonly TerminalOperatorType Empty = new();
        private static readonly ImmutableSortedDictionary<byte, TerminalOperatorType> _ValueObjectMap;

        /// <remarks>DecimalValue: 10; HexValue: 0x0A</remarks>
        public static readonly TerminalOperatorType FinancialInstitution;

        /// <remarks>DecimalValue: 20; HexValue: 0x14</remarks>
        public static readonly TerminalOperatorType Merchant;

        /// <remarks>DecimalValue: 30; HexValue: 0x1E</remarks>
        public static readonly TerminalOperatorType Cardholder;

        private const byte _FinancialInstitution = 10;
        private const byte _Merchant = 20;
        private const byte _Cardholder = 30;

        #endregion

        #region Constructor

        public TerminalOperatorType()
        { }

        /// <exception cref="TypeInitializationException"></exception>
        /// <exception cref="PlayInternalException"></exception>
        static TerminalOperatorType()
        {
            FinancialInstitution = new TerminalOperatorType(_FinancialInstitution);
            Merchant = new TerminalOperatorType(_Merchant);
            Cardholder = new TerminalOperatorType(_Cardholder);

            _ValueObjectMap = new Dictionary<byte, TerminalOperatorType>
            {
                {FinancialInstitution, FinancialInstitution}, {Merchant, Merchant}, {Cardholder, Cardholder}
            }.ToImmutableSortedDictionary();
        }

        private TerminalOperatorType(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public override TerminalOperatorType[] GetAll() => _ValueObjectMap.Values.ToArray();

        public override bool TryGet(byte value, out EnumObject<byte>? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out TerminalOperatorType? enumResult))
            {
                result = enumResult;

                return true;
            }

            result = null;

            return false;
        }

        private static byte ClearUnrelatedDigit(byte value) => (byte) ((byte) (value / 10) * 10);
        public static bool IsOperatorType(byte value, TerminalOperatorType operatorType) => ClearUnrelatedDigit(value) == operatorType;
        public static bool TryGet(byte value, out TerminalOperatorType result) => _ValueObjectMap.TryGetValue(value, out result);

        #endregion

        #region Equality

        public bool Equals(TerminalOperatorType x, TerminalOperatorType y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public override int GetHashCode() => 47533 * _Value.GetHashCode();
        public int GetHashCode(TerminalOperatorType obj) => obj.GetHashCode();

        public int CompareTo(TerminalOperatorType? other)
        {
            if (other is null)
                return 1;

            return _Value.CompareTo(other._Value);
        }

        #endregion

        #region Operator Overrides

        public static bool operator ==(TerminalOperatorType left, byte right)
        {
            if (left is null)
                return false;

            return left._Value == right;
        }

        public static bool operator ==(byte left, TerminalOperatorType right)
        {
            if (right is null)
                return false;

            return right._Value == left;
        }

        public static explicit operator byte(TerminalOperatorType terminalOperatorType) => terminalOperatorType._Value;

        /// <exception cref="DataElementParsingException"></exception>
        public static explicit operator TerminalOperatorType(byte terminalOperatorType)
        {
            if (!TryGet(terminalOperatorType, out TerminalOperatorType result))
            {
                throw new DataElementParsingException(nameof(TerminalOperatorType),
                    $"The {nameof(TerminalOperatorType)} could not be found from the number supplied to the argument: {nameof(terminalOperatorType)}");
            }

            return result;
        }

        public static bool operator !=(TerminalOperatorType left, byte right) => !(left == right);
        public static bool operator !=(byte left, TerminalOperatorType right) => !(left == right);

        #endregion
    }
}