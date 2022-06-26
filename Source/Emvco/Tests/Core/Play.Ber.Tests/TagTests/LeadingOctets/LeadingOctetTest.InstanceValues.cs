using Play.Ber.Identifiers;
using Play.Ber.Identifiers.Long;
using Play.Core.Extensions;

using Xunit;

namespace Play.Ber.Tests.TagTests.LeadingOctets;

public partial class LeadingOctetTests
{
    #region Instance Members

    [Fact]
    public void Byte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte initializationValue = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);

        Tag sut = new(initializationValue);

        Assert.Equal(sut.GetClassType(), expectedClass);
        Assert.Equal(sut.GetDataObject(), dataObjectType);
    }

    [Fact]
    public void ByteArray_WithPrivateConstructedLeadingOCtet_CreatesTagWithCorrectClass()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        byte[] testValue = new byte[] {leadingOctet, 31};

        Tag sut = new(testValue);

        Assert.Equal(sut.GetClassType(), expectedClass);
    }

    [Fact]
    public void ByteArray_WithPrivateConstructedLeadingOctet_CreatesTagWithDataObject()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        byte[] testValue = new byte[] {leadingOctet, 45};

        Tag sut = new(testValue);

        Assert.Equal(sut.GetDataObject(), dataObjectType);
    }

    [Fact]
    public void ByteArray_WithUniversalConstructedLeadingOctet_CreatesTagWithDataObject()
    {
        ClassTypes expectedClass = ClassTypes.Universal;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        byte[] testValue = new byte[] {leadingOctet, 47};

        Tag sut = new(testValue);

        Assert.Equal(sut.GetClassType(), expectedClass);
    }

    [Fact]
    public void ByteArray_WithContextSpecificConstructedLeadingOctet_CreatesTagWithDataObject()
    {
        ClassTypes expectedClass = ClassTypes.ContextSpecific;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        byte[] testValue = new byte[] {leadingOctet, 33};

        Tag sut = new(testValue);

        Assert.Equal(sut.GetClassType(), expectedClass);
    }

    [Fact]
    public void ByteArray_WithPrivatePrimitiveLeadingOctet_CreatesTagWithDataObject()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Primitive;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        byte[] testValue = new byte[] {leadingOctet, 34};

        Tag sut = new(testValue);

        Assert.Equal(sut.GetClassType(), expectedClass);
    }

    #endregion
}