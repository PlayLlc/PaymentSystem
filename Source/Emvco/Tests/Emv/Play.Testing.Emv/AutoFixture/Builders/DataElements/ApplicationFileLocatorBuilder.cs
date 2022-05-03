using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;
using Play.Randoms;

namespace Play.Testing.Emv;

public class ApplicationExpirationDateBuilder : PrimitiveValueSpecimenBuilder<ApplicationFileLocator>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationExpirationDateBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationExpirationDateBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationFileLocator>(ApplicationFileLocator.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0x08, 0x01, 0x03, 0x00, 0x10, 0x01, 0x01, 0x00};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(ApplicationFileLocator))
            return new NoSpecimen();

        int byteCount = Randomize.Integers.Byte(4, 248);
        byteCount -= byteCount % 4;

        return new ApplicationFileLocator(Randomize.Arrays.Bytes(byteCount));
    }

    #endregion
}