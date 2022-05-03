using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class ApplicationLabelBuilder : PrimitiveValueSpecimenBuilder<ApplicationLabel>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationLabelBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationLabelBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationLabel>(ApplicationLabel.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() =>
        new byte[]
        {
            0x56, 0x49, 0x53, 0x41, 0x20, 0x50, 0x52, 0x45, 0x50, 0x41,
            0x49, 0x44
        };

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationLabel))
            return new NoSpecimen();

        return new ApplicationLabel(Randomize.AlphaNumeric.Chars(Randomize.Integers.Int(1, 16)).AsSpan());
    }

    #endregion
}