using AutoFixture.Kernel;

using Play.Encryption.Certificates;
using Play.Icc.Exceptions;

namespace Play.Testing.Emv;

public class CertificateSerialNumberBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(CertificateSerialNumberBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="IccProtocolException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(CertificateSerialNumber))
            return new NoSpecimen();

        byte[] value = new byte[3];
        new Random().NextBytes(value);

        return new CertificateSerialNumber(value);
    }

    #endregion
}