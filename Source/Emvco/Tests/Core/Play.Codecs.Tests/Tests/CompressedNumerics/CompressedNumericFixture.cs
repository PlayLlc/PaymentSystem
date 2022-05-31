using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Randoms;

namespace Play.Codecs.Tests.Tests.CompressedNumerics
{
    internal class CompressedNumericFixture
    {
        #region Static Metadata

        private static readonly Random _Random = new();

        #endregion

        #region Instance Members

        /// <exception cref="OverflowException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<object[]> GetRandomBytes(int count, int minLength, int maxLength)
        {
            if (minLength > maxLength)
                throw new ArgumentOutOfRangeException(nameof(minLength));

            for (int i = 0; i < count; i++)
                yield return new object[] {Randomize.CompressedNumeric.Bytes(_Random.Next(minLength, maxLength))};
        }

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IEnumerable<object[]> GetRandomString(int count, int minLength, int maxLength)
        {
            if (minLength > maxLength)
                throw new ArgumentOutOfRangeException(nameof(minLength));

            for (int i = 0; i < count; i++)
            {
                var a = _Random.Next(minLength, maxLength);

                yield return new object[] {Randomize.CompressedNumeric.String(a - (a % 2))};
            }
        }

        #endregion
    }
}