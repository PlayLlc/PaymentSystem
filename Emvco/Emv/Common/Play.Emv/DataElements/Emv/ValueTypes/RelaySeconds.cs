using System;

using Play.Globalization.Time.Seconds;

namespace Play.Emv.DataElements
{
    /// <summary>
    ///     A <see cref="RelaySecond" /> is a unit of time equal to 100 microseconds. It's used along with the Relay Resistance
    ///     Protocol to to help ensure that a 'Relay Attack' is not taking place
    /// </summary>
    public readonly record struct RelaySeconds
    {
        #region Static Metadata

        public static readonly RelaySeconds Zero = new(0);
        public const int Precision = 10000;

        #endregion

        #region Instance Values

        private readonly long _Value;

        #endregion

        #region Constructor

        /// <remarks>
        ///     The <paramref name="value" /> must be 922337203685477 or less in value
        /// </remarks>
        /// <param name="value"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public RelaySeconds(long value)
        {
            _Value = value;
        }

        public RelaySeconds(uint value)
        {
            _Value = value;
        }

        public RelaySeconds(ushort value)
        {
            _Value = value;
        }

        public RelaySeconds(byte value)
        {
            _Value = value;
        }

        public RelaySeconds(TimeSpan value)
        {
            _Value = value.Ticks / (Ticks.Precision / Precision);
        }

        public RelaySeconds(Deciseconds value)
        {
            _Value = (long) value * (Precision / Deciseconds.Precision);
        }

        public RelaySeconds(Seconds value)
        {
            _Value = (long) value * (Precision / Seconds.Precision);
        }

        public RelaySeconds(Milliseconds value)
        {
            _Value = (long) value * (Precision / Milliseconds.Precision);
        }

        #endregion

        #region Instance Members

        public TimeSpan AsTimeSpan() => new(_Value * (Ticks.Precision / Precision));

        #endregion

        #region Equality

        public bool Equals(RelaySeconds other) => _Value == other._Value;
        public bool Equals(TimeSpan other) => AsTimeSpan() == other;
        public static bool Equals(RelaySeconds x, RelaySeconds y) => x.Equals(y);
        public bool Equals(long other) => _Value == other;

        public override int GetHashCode()
        {
            const int hash = 297581;

            return hash + _Value.GetHashCode();
        }

        #endregion

        #region Operator Overrides

        public static RelaySeconds operator *(RelaySeconds left, RelaySeconds right) => new(left._Value * right._Value);
        public static RelaySeconds operator /(RelaySeconds left, RelaySeconds right) => new(left._Value / right._Value);
        public static RelaySeconds operator -(RelaySeconds left, RelaySeconds right) => new(left._Value - right._Value);
        public static RelaySeconds operator +(RelaySeconds left, RelaySeconds right) => new(left._Value + right._Value);
        public static bool operator >(long left, RelaySeconds right) => left > right._Value;
        public static bool operator <(long left, RelaySeconds right) => left < right._Value;
        public static bool operator >=(long left, RelaySeconds right) => left >= right._Value;
        public static bool operator <=(long left, RelaySeconds right) => left <= right._Value;
        public static bool operator ==(long left, RelaySeconds right) => left == right._Value;
        public static bool operator !=(long left, RelaySeconds right) => left != right._Value;
        public static bool operator >(RelaySeconds left, long right) => left._Value > right;
        public static bool operator <(RelaySeconds left, long right) => left._Value < right;
        public static bool operator >=(RelaySeconds left, long right) => left._Value >= right;
        public static bool operator <=(RelaySeconds left, long right) => left._Value <= right;
        public static bool operator ==(RelaySeconds left, long right) => left._Value == right;
        public static bool operator !=(RelaySeconds left, long right) => left._Value != right;
        public static bool operator ==(RelaySeconds left, TimeSpan right) => left.Equals(right);
        public static bool operator ==(TimeSpan left, RelaySeconds right) => right.Equals(left);
        public static explicit operator long(RelaySeconds value) => value._Value;
        public static bool operator >(RelaySeconds left, RelaySeconds right) => left._Value > right._Value;
        public static bool operator >(RelaySeconds left, TimeSpan right) => left.AsTimeSpan() > right;
        public static bool operator >(TimeSpan left, RelaySeconds right) => right.AsTimeSpan() > left;
        public static bool operator >=(RelaySeconds left, RelaySeconds right) => left._Value >= right._Value;
        public static bool operator >=(RelaySeconds left, TimeSpan right) => left.AsTimeSpan() >= right;
        public static bool operator >=(TimeSpan left, RelaySeconds right) => right.AsTimeSpan() >= left;
        public static implicit operator TimeSpan(RelaySeconds value) => value.AsTimeSpan();
        public static implicit operator RelaySeconds(TimeSpan value) => new(value);
        public static implicit operator RelaySeconds(int value) => new((uint) value);
        public static bool operator !=(RelaySeconds left, TimeSpan right) => !left.Equals(right);
        public static bool operator !=(TimeSpan left, RelaySeconds right) => !right.Equals(left);
        public static bool operator <(RelaySeconds left, RelaySeconds right) => left._Value < right._Value;
        public static bool operator <(RelaySeconds left, TimeSpan right) => left.AsTimeSpan() < right;
        public static bool operator <(TimeSpan left, RelaySeconds right) => right.AsTimeSpan() < left;
        public static bool operator <=(RelaySeconds left, RelaySeconds right) => left._Value <= right._Value;
        public static bool operator <=(RelaySeconds left, TimeSpan right) => left.AsTimeSpan() <= right;
        public static bool operator <=(TimeSpan left, RelaySeconds right) => right.AsTimeSpan() <= left;
        public static implicit operator RelaySeconds(Seconds value) => new(value);
        public static implicit operator RelaySeconds(Deciseconds value) => new(value);
        public static implicit operator Microseconds(RelaySeconds value) => new(value);

        #endregion
    }
}