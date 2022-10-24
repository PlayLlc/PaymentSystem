using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Ber.Tags;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class UnpredictableNumberDataObjectListTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 14, 5, 23, 4, 30, 5, 0x9F, 0x37, 3 };
    private static readonly Tag[] _DefaultContentTags = { new(14), new(23), new(30), UnpredictableNumber.Tag };

    public UnpredictableNumberDataObjectListTestTlv() : base(_DefaultContentOctets) { }

    public UnpredictableNumberDataObjectListTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => UnpredictableNumberDataObjectList.Tag;

    public Tag[] GetTags() => _DefaultContentTags;

    public void SetupTlvTagsForGivenDb(Mock<ITlvReaderAndWriter> database)
    {
        for (byte i = 0; i < _DefaultContentTags.Length; i++)
        {
            database.Setup(m => m.IsKnown(_DefaultContentTags[i])).Returns(true);
            database.Setup(m => m.IsPresent(_DefaultContentTags[i])).Returns(true);
            database.Setup(m => m.IsPresentAndNotEmpty(_DefaultContentTags[i])).Returns(true);
        }
    }

    public DataObjectListResult SetupValuesForTags(Mock<ITlvReaderAndWriter> database, IFixture fixture)
    {
        List<TagLengthValue> result = new List<TagLengthValue>();

        foreach (Tag tag in _DefaultContentTags)
        {
            TestPrimitive primitive = fixture.Create<TestPrimitive>();

            database.Setup(m => m.Get(tag)).Returns(primitive);

            result.Add(new TagLengthValue(tag, primitive.EncodeValue()));
        }

        return new DataObjectListResult(result.ToArray());
    }
}
