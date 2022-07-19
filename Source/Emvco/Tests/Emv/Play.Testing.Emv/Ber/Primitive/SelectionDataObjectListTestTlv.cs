using AutoFixture;

using Moq;

using Play.Ber.DataObjects;
using Play.Ber.Identifiers;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Ber.Primitive;

public class SelectionDataObjectListTestTlv : TestTlv
{
    private static readonly byte[] _DefaultContentOctets = { 0x9F, 0x02, 3, 0x8B, 5, 0x9F, 0x1A, 4, 0x5F, 0x2A, 5 };
    private static readonly Tag[] _DefaultContentTags = { AmountAuthorizedNumeric.Tag, PoiInformation.Tag, TerminalCountryCode.Tag, TransactionCurrencyCode.Tag };

    public SelectionDataObjectListTestTlv() : base(_DefaultContentOctets) { }

    public SelectionDataObjectListTestTlv(byte[] contentOctets) : base(contentOctets)
    {
    }

    public override Tag GetTag() => SelectionDataObjectList.Tag;

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
