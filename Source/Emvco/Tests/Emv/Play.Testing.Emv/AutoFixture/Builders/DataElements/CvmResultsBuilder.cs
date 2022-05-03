using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class CvmResultsBuilder : PrimitiveValueSpecimenBuilder<CvmResults>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(CvmResultsBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public CvmResultsBuilder() : base(new DefaultPrimitiveValueSpecimen<CvmResults>(CvmResults.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x54, 0x44, 0x43, 0x20, 0x42, 0x4C, 0x41, 0x43, 0x4B, 0x20,
            0x55, 0x4E, 0x4C, 0x49, 0x4D, 0x49, 0x54, 0x45, 0x44, 0x20,
            0x56, 0x49, 0x53, 0x41, 0x20, 0x20
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(CvmResults))
            return new NoSpecimen();

        return new CvmResults(Randomize.Integers.UInt());
    }

    #endregion
}