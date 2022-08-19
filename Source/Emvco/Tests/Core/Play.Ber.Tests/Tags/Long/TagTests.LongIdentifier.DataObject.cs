using Play.Ber.Tags;
using Play.Ber.Tags.Long;

using Xunit;

namespace Play.Ber.Tests.Long;

public partial class TagTests
{
    #region Instance Members

    [Fact]
    public void ByteArray_WithPrivateConstructedLeadingOctet_CreatesTagWithDataObject()
    {
        ClassTypes expectedClass = ClassTypes.Private;
        DataObjectTypes dataObjectType = DataObjectTypes.Constructed;

        byte leadingOctet = (byte) ((byte) expectedClass | (byte) dataObjectType | LongIdentifier.LongIdentifierFlag);
        byte[] testValue = {leadingOctet, 45};

        Tag sut = new(testValue);

        Assert.Equal(sut.GetDataObject(), dataObjectType);
    }

    #endregion
}