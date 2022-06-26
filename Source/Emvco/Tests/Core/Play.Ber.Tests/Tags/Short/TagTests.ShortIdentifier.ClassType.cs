using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Short;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.Tags.__Temp
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void Byte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.ContextSpecific);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.ContextSpecific);
        }

        [Fact]
        public void Byte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ShortIdentifier.TagNumber.MaxValue.GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.Application);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Application);
        }

        [Fact]
        public void Byte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ShortIdentifier.TagNumber.MaxValue.SetBits((byte) ClassTypes.Private);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Private);
        }

        [Fact]
        public void Byte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ShortIdentifier.TagNumber.MaxValue;

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Universal);
        }

        [Fact]
        public void RandomByte_WithApplicationClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.Application);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Application);
        }

        [Fact]
        public void RandomByte_WithContextSpecificClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.ContextSpecific);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.ContextSpecific);
        }

        [Fact]
        public void RandomByte_WithPrivateClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.ContextSpecific);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.ContextSpecific);
        }

        [Fact]
        public void RandomByte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
        {
            byte testValue = ((byte) _Random.Next(0, byte.MaxValue)).GetMaskedValue(Bits.Eight, Bits.Seven).SetBits((byte) ClassTypes.Universal);

            Tag sut = new(testValue);

            ClassTypes result = sut.GetClass();

            Assert.Equal(result, ClassTypes.Universal);
        }

        #endregion
    }
}