//using System;
//using System.Collections.Generic;
//using System.Text;
//using Play.Core.Codecs;
//using Play.Core.Extensions;
//using Play.Core.Iso8825.Ber.Identifiers;
//using Play.Core.Iso8825.Ber.Identifiers.Specifications;
//using Play.Core.Iso8825.Tests.Ber.TagTests.SubsequentOctets.TestData;
//using Xunit;

//namespace Play.Core.Iso8825.Tests.Ber.TagTests.SubsequentOctets
//{
//    public partial class SubsequentOctetsTests
//    {
//        [Fact]
//        public void ValidBytes_WhenInstantiatingSubsequentBytes_SerializesToTHeCorrectValue()
//        {
//            var bytes = new byte[] {0xA6, 0x4B};

//            var sut = new Iso8825.Ber.Identifiers.SubsequentOctets(bytes.AsSpan());

//            Assert.Equal(bytes, sut.Serialize());
//        }

//        [Fact]
//        public void RandomValidBytes_WhenInstantiatingSubsequentBytes_SerializesToTHeCorrectValue()
//        {
//            byte[] bytes = SubsequentOctetsTestValueFactory.GetValidSubsequentOctetsValue(_Random);

//            var sut = new Iso8825.Ber.Identifiers.SubsequentOctets(bytes.AsSpan());

//            Assert.Equal(bytes, sut.Serialize());
//        }

//    }
//}

