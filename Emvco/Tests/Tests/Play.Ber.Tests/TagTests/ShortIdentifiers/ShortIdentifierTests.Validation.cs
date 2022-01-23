//using Play.Core.Extensions;
//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Ber.Identifiers.Specifications;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.ShortIdentifiers
//{
//    public partial class TagFactoryTests
//    {
//        [Fact]
//        public void Byte_RandomTagNumberLessThan31_IsValidShortIdentifier()
//        {
//            var testValue = (byte) _Random.Next(0, Spec.ShortIdentifier.TagNumber.MaxLength);
//            var result = ShortIdentifier.IsValid(testValue);

//            Assert.True(result);
//        }

//        [Fact]
//        public void Byte_RandomValueWithLongIdentifierFlag_IsNotValidShortIdentifier()
//        {
//            var testValue =
//                ((byte) _Random.Next(0, byte.MaxValue)).SetBits(Spec.LeadingOctet.LongIdentifierFlag);

//            var result = ShortIdentifier.IsValid(testValue);

//            Assert.False(result);
//        }

//        [Fact]
//        public void Byte_TagNumberLessThan31_IsValidShortIdentifier()
//        {
//            var testValue = Spec.ShortIdentifier.TagNumber.MaxLength;
//            var result = ShortIdentifier.IsValid(testValue);

//            Assert.True(result);
//        }

//        [Fact]
//        public void Byte_ValueWithLongIdentifierFlag_IsNotValidShortIdentifier()
//        {
//            var testValue =
//                ((byte) _Random.Next(Spec.ShortIdentifier.TagNumber.MaxLength + 1, byte.MaxValue)).SetBits(
//                    Spec.LeadingOctet.LongIdentifierFlag);
//            var result = ShortIdentifier.IsValid(testValue);

//            Assert.False(result);
//        }
//    }
//}

