using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;

using Xunit;

namespace Play.Ber.Tests.Long
{
    public partial class TagTests
    {
        #region Instance Members

        [Fact]
        public void ByteArray_WithPrivateConstructedLeadingOctet_CreatesTagWithCorrectClass()
        {
            ClassTypes expectedClass = ClassTypes.Private;
            DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

            byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
            byte[] testValue = {leadingOctet, 31};

            Tag sut = new(testValue);

            Assert.Equal(sut.GetClassType(), expectedClass);
        }

        [Fact]
        public void ByteArray_WithUniversalConstructedLeadingOctet_CreatesTagWithDataObject()
        {
            ClassTypes expectedClass = ClassTypes.Universal;
            DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

            byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
            byte[] testValue = {leadingOctet, 47};

            Tag sut = new(testValue);

            Assert.Equal(sut.GetClassType(), expectedClass);
        }

        [Fact]
        public void ByteArray_WithContextSpecificConstructedLeadingOctet_CreatesTagWithDataObject()
        {
            ClassTypes expectedClass = ClassTypes.ContextSpecific;
            DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

            byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
            byte[] testValue = {leadingOctet, 33};

            Tag sut = new(testValue);

            Assert.Equal(sut.GetClassType(), expectedClass);
        }

        [Fact]
        public void ByteArray_WithPrivatePrimitiveLeadingOctet_CreatesTagWithDataObject()
        {
            ClassTypes expectedClass = ClassTypes.Private;
            DataObjectTypes dataObjectType = DataObjectTypes.Primitive;

            byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
            byte[] testValue = {leadingOctet, 34};

            Tag sut = new(testValue);

            Assert.Equal(sut.GetClassType(), expectedClass);
        }

        #endregion
    }
}