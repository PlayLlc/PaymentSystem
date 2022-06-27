using Play.Ber.Identifiers;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.Short
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void RandomByte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = ((byte) Tests.TagTests._Random.Next(0, byte.MaxValue)).SetBits((byte) DataObjectTypes.Constructed);

            Tag sut = new(testValue);

            DataObjectTypes result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Constructed);
        }

        [Fact]
        public void RandomByte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = ((byte) Tests.TagTests._Random.Next(0, byte.MaxValue)).GetMaskedValue((byte) Bits.Six);

            Tag sut = new(testValue);

            DataObjectTypes result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Primitive);
        }

        [Fact]
        public void Byte_WithConstructedFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = 0b_01111011;

            Tag sut = new(testValue);

            DataObjectTypes result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Constructed);
        }

        [Fact]
        public void Byte_WithPrimitiveFlag_CreatesTagWithCorrectDataObjectType()
        {
            byte testValue = 0b_01011011;

            Tag sut = new(testValue);

            DataObjectTypes result = sut.GetDataObject();

            Assert.Equal(result, DataObjectTypes.Primitive);
        }

        #endregion
    }
}