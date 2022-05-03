using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class AmountOtherNumericBuilder : PrimitiveValueSpecimenBuilder<AmountOtherNumeric>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(AmountOtherNumericBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public AmountOtherNumericBuilder() : base(
        new DefaultPrimitiveValueSpecimen<AmountOtherNumeric>(AmountOtherNumeric.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0x00, 0x00, 0x00, 0x00, 0x00, 0x01};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(AmountOtherNumeric))
            return new NoSpecimen();

        return new AmountOtherNumeric(Randomize.Integers.ULong());
    }

    #endregion
}