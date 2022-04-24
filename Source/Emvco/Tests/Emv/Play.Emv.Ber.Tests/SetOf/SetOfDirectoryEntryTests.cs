using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoMoq;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Templates;
using Play.Emv.Ber.ValueTypes;
using Play.Emv.Identifiers;
using Play.Testing.BaseTestClasses;
using Play.Testing.Emv;
using Play.Testing.Emv.Ber.Constructed;

using Xunit;

namespace Play.Emv.Ber.Tests.SetOf
{
    [Trait("Type", "Unit")]
    public class SetOfDirectoryEntryTests : TestBase
    {
        #region Instance Values

        private readonly IFixture _Fixture;

        #endregion

        #region Constructor

        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="BerParsingException"></exception>
        public SetOfDirectoryEntryTests()
        {
            _Fixture = new EmvFixture().Create();
        }

        #endregion

        #region Instance Members

        /// <exception cref="BerParsingException"></exception>
        [Fact]
        public void BerEncoding_DeserializingTemplate_CreatesConstructedValue()
        {
            DirectoryEntryTestTlv testData = new();

            DirectoryEntry sut = DirectoryEntry.Decode(testData.EncodeTagLengthValue());
            Assert.NotNull(sut);
        }

        [Fact]
        public void BerEncoding_DeserializingSetOfDirectoryEntry_Creates()
        {
            // var directoryEntries = _Fixture.Create<DirectoryEntry>();

            var aaa = new byte[] {0x5F, 0x55, 0x02, 0x55, 0x53};
            var bb = IssuerCountryCode.Decode(aaa.AsSpan());

            //SetOf<DirectoryEntry> testData = _Fixture.Create<SetOf<DirectoryEntry>>(); 
            //var encodedValue = testData.EncodeValue(EmvCodec.GetCodec()); 
            //  Assert.NotNull(directoryEntries);
        }

        #endregion
    }
}