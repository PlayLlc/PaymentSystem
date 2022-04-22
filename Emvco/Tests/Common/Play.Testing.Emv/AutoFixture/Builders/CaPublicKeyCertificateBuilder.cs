using AutoFixture.Kernel;

using Play.Emv.Security;
using Play.Icc.FileSystem.DedicatedFiles;
using Play.Emv.Ber.DataElements;

namespace Play.Testing.Emv.Contactless.AutoFixture.Buildersdd
{
    //public class CaPublicKeyCertificateBuilder : SpecimenBuilder
    //{
    //    #region Static Metadata

    //    public static readonly SpecimenBuilderId Id = new(nameof(CaPublicKeyCertificateBuilder));

    //    #endregion

    //    #region Instance Members

    //    public override SpecimenBuilderId GetId() => Id;

    //    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    //    public override object Create(object request, ISpecimenContext context)
    //    {
    //        Type? type = request as Type;

    //        if (type == null)
    //            return new NoSpecimen();

    //        if (type != typeof(CaPublicKeyCertificate))
    //            return new NoSpecimen();

    //        return new CertificateAuthorityDataset(DedicatedFileName.ProximityPaymentSystemEnvironment.GetRegisteredApplicationProviderIdentifier(),
    //            Array.Empty<CaPublicKeyCertificate>());
    //    }

    //    #endregion
    //}
}