using System;
using System.Diagnostics.CodeAnalysis;

using Play.Core.Extensions;

namespace Play.Emv.Security.Encryption.Mac
{
    // TODO: This isn't needed from the terminal side. will save this and maybe use for testing responses
    // TODO: from the ICC one day
    public readonly struct MessageAuthenticationCode
    {
        #region Static Metadata

        private const byte _MinByteCount = 4;
        private const byte _MaxByteCount = 8;

        #endregion

        #region Instance Values
        private readonly byte[] _Value;
        #endregion

        #region Constructor
        public MessageAuthenticationCode(ReadOnlySpan<byte> value)
        {
            if (value.Length < _MinByteCount)
                throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be at least {_MinByteCount} bytes to initialize a {nameof(MessageAuthenticationCode)}");
            if (value.Length > _MaxByteCount)
                throw new ArgumentOutOfRangeException(nameof(value), $"The argument {nameof(value)} must be less than {_MaxByteCount} bytes to initialize a {nameof(MessageAuthenticationCode)}");
 
            _Value = value.ToArray();
        }
        #endregion

        #region Instance Members

        public byte[] AsByteArray() => _Value.CopyValue();

        #endregion

        #region IEquatable

        public bool Equals(MessageAuthenticationCode other) => other._Value == _Value;

        #endregion

        #region IEqualityComparer

        public bool Equals(MessageAuthenticationCode x, MessageAuthenticationCode y) => x.Equals(y);

        public int GetHashCode(MessageAuthenticationCode other) => other.GetHashCode();

        #endregion

        #region Object Overrides

        public override bool Equals([AllowNull] object obj) => obj is MessageAuthenticationCode code && Equals(code);

        public override int GetHashCode()
        {
            return unchecked(40933 * _Value.GetHashCode());
        }

        #endregion

    }
}
