using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;
using Play.Ber.Identifiers.Short;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.Tags.__Temp
{
    public partial class TagTests
    {
        #region Instance Members

        /// <exception cref="BerParsingException"></exception>
        [Fact]
        public void Byte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
        {
            byte tagNumber = 0b_00000101;
            byte testValue = (byte) (0b_10100000 | tagNumber);

            Tag sut = new(testValue);

            ushort result = sut.GetTagNumber();

            Assert.Equal(result, tagNumber);
        }

        /// <exception cref="BerParsingException"></exception>
        [Fact]
        public void RandomByte_WithValueLessThan31_CreatesTagWithCorrectTagNumber()
        {
            byte tagNumber = (byte) _Random.Next(0, ShortIdentifier.TagNumber.MaxValue);
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(LongIdentifier.LongIdentifierFlag).SetBits(tagNumber);

            Tag sut = new(testValue);

            ushort result = sut.GetTagNumber();

            Assert.Equal(result, tagNumber);
        }

        #endregion
    }
}