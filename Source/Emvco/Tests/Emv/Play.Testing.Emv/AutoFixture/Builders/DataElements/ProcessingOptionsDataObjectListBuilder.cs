using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;

namespace Play.Testing.Emv;

public class ProcessingOptionsDataObjectListBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly byte[] RawTagLengthValue = new byte[]
    {
        0x9F, 0x38, 0x1B, 0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F,
        0x03, 0x06, 0x9F, 0x1A, 0x02, 0x95, 0x05, 0x5F, 0x2A, 0x02,
        0x9A, 0x03, 0x9C, 0x01, 0x9F, 0x37, 0x04, 0x9F, 0x4E, 0x14
    };

    public static readonly byte[] RawValue = new byte[]
    {
        0x9F, 0x66, 0x04, 0x9F, 0x02, 0x06, 0x9F, 0x03, 0x06, 0x9F,
        0x1A, 0x02, 0x95, 0x05, 0x5F, 0x2A, 0x02, 0x9A, 0x03, 0x9C,
        0x01, 0x9F, 0x37, 0x04, 0x9F, 0x4E, 0x14
    };

    public static readonly SpecimenBuilderId Id = new(nameof(ProcessingOptionsDataObjectListBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ProcessingOptionsDataObjectList))
            return new NoSpecimen();

        return ProcessingOptionsDataObjectList.Decode(RawTagLengthValue.AsSpan());
    }

    #endregion
}