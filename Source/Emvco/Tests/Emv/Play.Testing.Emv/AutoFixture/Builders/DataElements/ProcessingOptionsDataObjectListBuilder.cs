using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Enums;
using Play.Randoms;

namespace Play.Testing.Emv;

public class ProcessingOptionsDataObjectListBuilder : PrimitiveValueSpecimenBuilder<ProcessingOptionsDataObjectList>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ProcessingOptionsDataObjectList));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ProcessingOptionsDataObjectListBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ProcessingOptionsDataObjectList>(ProcessingOptionsDataObjectList.Decode(GetContentOctets().AsSpan()),
            GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x9F, 0x66, 0x04, // PUNATC(Track2)
            0x9F, 0x02, 0x06, // Amount Authorized (Numeric)
            0x9F, 0x03, 0x06, // Amount Other (Numeric)
            0x9F, 0x1A, 0x02, // Terminal Country Code
            0x95, 0x05, // Terminal Verification Results

            0x5F, 0x2A, 0x02, // Transaction Currency Code
            0x9A, 0x03, // Transaction Date
            0x9C, 0x01, // Transaction Type

            0x9F, 0x37, 0x04, // Unpredictable Number
            0x9F, 0x4E, 0x14 // Merchant Name and Location
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ProcessingOptionsDataObjectList))
            return new NoSpecimen();

        return GetDefault();
    }

    #endregion
}