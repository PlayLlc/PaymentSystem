using AutoFixture.Kernel;

using Play.Ber.Exceptions;
using Play.Codecs.Exceptions;
using Play.Emv.Ber;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Randoms;

namespace Play.Testing.Emv;

public class DedicatedFileNameBuilder : PrimitiveValueSpecimenBuilder<DedicatedFileName>
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(DedicatedFileNameBuilder));

    #endregion

    #region Constructor

    /// <exception cref="BerParsingException"></exception>
    /// <exception cref="CodecParsingException"></exception>
    public DedicatedFileNameBuilder() : base(
        new DefaultPrimitiveValueSpecimen<DedicatedFileName>(DedicatedFileName.Decode(GetContentOctets().AsSpan(), EmvCodec.GetCodec()), GetContentOctets()))
    { }

    #endregion

    #region Instance Members

    private static byte[] GetContentOctets() => new byte[] {0x80, 0x07, 0xA0, 0x00, 0x00, 0x00, 0x98, 0x08, 0x40};
    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(DedicatedFileName))
            return new NoSpecimen();

        return new DedicatedFileName(Randomize.Arrays.Bytes(Randomize.Integers.Int(5, 16)));
    }

    #endregion
}