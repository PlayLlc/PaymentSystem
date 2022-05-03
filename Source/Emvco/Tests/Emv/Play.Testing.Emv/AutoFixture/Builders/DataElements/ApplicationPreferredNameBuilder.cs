using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class ApplicationPreferredNameBuilder : PrimitiveValueSpecimenBuilder<ApplicationPreferredName>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationPreferredNameBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationPreferredNameBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationPreferredName>(ApplicationPreferredName.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x56, 0x49, 0x53, 0x41, 0x20, 0x43, 0x52, 0x45, 0x44, 0x49,
            0x54, 0x4F
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationPreferredName))
            return new NoSpecimen();

        return new ApplicationPreferredName(Randomize.AlphaNumeric.Chars(Randomize.Integers.Int(1, 16)).AsSpan());
    }

    #endregion
}