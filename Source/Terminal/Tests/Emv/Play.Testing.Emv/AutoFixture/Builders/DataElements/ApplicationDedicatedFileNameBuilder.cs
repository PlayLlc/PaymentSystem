using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class ApplicationDedicatedFileNameBuilder : PrimitiveValueSpecimenBuilder<ApplicationDedicatedFileName>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationDedicatedFileNameBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationDedicatedFileNameBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationDedicatedFileName>(ApplicationDedicatedFileName.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationDedicatedFileName))
            return new NoSpecimen();

        return new ApplicationDedicatedFileName(Randomize.Integers.BigInteger(5, 16));
    }

    #endregion
}