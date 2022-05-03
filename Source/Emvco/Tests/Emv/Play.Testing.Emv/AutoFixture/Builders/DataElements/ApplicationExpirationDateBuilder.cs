using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Globalization.Time;
using Play.Randoms;
using Play.Testing.Emv;

namespace Play.Testing.Emv;

public class ApplicationExpirationDateBuilder : PrimitiveValueSpecimenBuilder<ApplicationExpirationDate>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationExpirationDateBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationExpirationDateBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationExpirationDate>(ApplicationExpirationDate.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0X12, 0X21, 0X22};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationExpirationDate))
            return new NoSpecimen();

        return new ApplicationExpirationDate(ShortDate.Today.AsYyMm());
    }

    #endregion
}