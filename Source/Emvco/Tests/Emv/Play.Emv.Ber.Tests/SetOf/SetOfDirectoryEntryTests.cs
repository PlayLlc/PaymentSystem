using System;

using AutoFixture;

using Play.Ber.DataObjects;
using Play.Ber.Exceptions;
using Play.Emv.Ber.Templates;
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
        public void BerEncoding_DeserializingSetOfDirectoryEntry_InitializesCorrectly()
        {
            SetOf<DirectoryEntry> testData = _Fixture.Create<SetOf<DirectoryEntry>>();
            Assert.NotNull(testData);
        }

        [Fact]
        public void BerEncoding_EncodingSetOfDirectoryEntry_SerializesExpectedValue()
        {
            byte[] expected = DirectoryEntryTestTlv.GetRawTagLengthValue();
            DirectoryEntry sut = DirectoryEntry.Decode(expected);
            byte[] actual = sut.EncodeTagLengthValue();

            Assertion(() => Assert.Equal(expected, actual), Build.Equals.Message(expected, actual));
        }

        #endregion
    }
}