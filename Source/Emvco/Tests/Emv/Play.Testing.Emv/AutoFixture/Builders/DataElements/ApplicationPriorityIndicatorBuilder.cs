using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv;

public class ApplicationPriorityIndicatorBuilder : PrimitiveValueSpecimenBuilder<ApplicationPriorityIndicator>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(ApplicationPriorityIndicator));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public ApplicationPriorityIndicatorBuilder() : base(
        new DefaultPrimitiveValueSpecimen<ApplicationPriorityIndicator>(ApplicationPriorityIndicator.Decode(GetContentOctets().AsSpan()), GetContentOctets()))
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

        if (type != typeof(ApplicationPriorityIndicator))
            return new NoSpecimen();

        return new ApplicationPriorityIndicator((byte) _Random.Next(1, 7));
    }

    #endregion
}