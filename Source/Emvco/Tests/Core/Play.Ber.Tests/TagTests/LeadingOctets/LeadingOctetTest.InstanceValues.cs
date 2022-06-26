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

    #endregion
}