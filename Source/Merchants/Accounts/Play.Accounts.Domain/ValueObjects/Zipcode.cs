using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.ValueObjects
{
    public record Zipcode : ValueObject<string>
    {
        #region Constructor

        /// <exception cref="ValueObjectException"></exception>
        public Zipcode(string value) : base(value[..5])
        {
            if (value.Length < 5)
                throw new ValueObjectException($"The {nameof(Email)} provided was invalid: [{value}]");

            ValidateDigitRange(value[0..5]);
        }

        #endregion

        #region Instance Members

        /// <exception cref="ValueObjectException"></exception>
        private static void ValidateDigitRange(ReadOnlySpan<char> value)
        {
            for (int i = 0; i < value.Length; i++)
                if (!char.IsDigit(value[i]))
                    throw new ValueObjectException($"The {nameof(Zipcode)} contained an invalid character: [{value[i]}]");
        }

        #endregion
    }
}