using System;

using Play.Ber.Identifiers;
using Play.Ber.Tests.TestData;
using Play.Testing.BaseTestClasses;

using Xunit;

namespace Play.Ber.Tests
{
    public partial class TagTests : TestBase
    {
        #region Static Metadata

        private static readonly Random _Random = new();

        #endregion

        #region Constructor

        static TagTests()
        { }

        #endregion

        #region Instance Members

        [Fact]
        public void RandomShortIdentifierComponentParts_WhenInitializing_CreatesByteWithCorrectValue()
        {
            ClassTypes? expectedClassType = ShortIdentifierTestValueFactory.GetClassType(_Random);
            DataObjectTypes? expectedDataObjectType = ShortIdentifierTestValueFactory.GetDataObjectType(_Random);
            byte expectedTagNumber = ShortIdentifierTestValueFactory.GetTagNumber(_Random);

            byte initializationValue = (byte) ((byte) expectedClassType | (byte) expectedDataObjectType | expectedTagNumber);

            Tag sut = new(initializationValue);

            Assert.Equal(expectedClassType, sut.GetClass());
            Assert.Equal(expectedDataObjectType, sut.GetDataObject());
            Assert.Equal(expectedTagNumber, sut.GetTagNumber());
        }

        #endregion
    }
}