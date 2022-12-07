using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class ApplicationInterchangeProfileBuilder : PrimitiveValueSpecimenBuilder<ApplicationInterchangeProfile>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationInterchangeProfileBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationInterchangeProfileBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationInterchangeProfile>(ApplicationInterchangeProfile.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0x1C, 0x00};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationInterchangeProfile))
            return new NoSpecimen();

        int byteCount = Randomize.Integers.Byte(4, 248);
        byteCount -= byteCount % 4;

        return new ApplicationInterchangeProfile(Randomize.Integers.UShort());
    }

    #endregion
}