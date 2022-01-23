namespace Play.Ber.Tests.TagTests.LeadingOctets;

public class LeadingOctetTests
{
    //[Fact]
    //public void Byte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
    //{
    //    var expectedClass = (ClassType) 0b11000000;
    //    var dataObjectType = (DataObjectType) 0b00100000;

    //    var initializationValue = (byte) ((byte) expectedClass | (byte) dataObjectType |
    //                                      LongIdentifier.LongIdentifierFlag);

    //    var sut = new LeadingOctet(initializationValue);

    //    Assert.Equal(sut.GetClassType(), expectedClass);
    //    Assert.Equal(sut.DataObject, dataObjectType);
    //}

    //[Fact]
    //public void RandomByte_WithUniversalClassFlag_CreatesTagWithCorrectClass()
    //{
    //    var testValue = ((byte) _Random.Next(0, byte.MaxValue))
    //        .GetMaskedByte(Spec.LeadingOctet.LongIdentifierFlag)
    //        .SetBits(Spec.LeadingOctet.LongIdentifierFlag);

    //    var sut = new LeadingOctet(testValue);

    //    Assert.Equal(sut.GetClassType(), (ClassType) testValue.GetMaskedByte(0b00111111));
    //    Assert.Equal(sut.DataObject, (DataObjectType) testValue.GetMaskedByte(0b11011111));
    //}
}