using AutoFixture.Kernel;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Security;
using Play.Icc.FileSystem.DedicatedFiles;

namespace Play.Testing.Emv.Contactless.AutoFixture;

//public class CertificateAuthorityDatasetBuilder : SpecimenBuilder
//{
//    #region Static Metadata

//    public static readonly SpecimenBuilderId Id = new(nameof(CertificateAuthorityDatasetBuilder));

//    #endregion

//    #region Instance Members

//    public override SpecimenBuilderId GetId() => Id;

//    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
//    public override object Create(object request, ISpecimenContext context)
//    {
//        Type? type = request as Type;

//        if (type == null)
//            return new NoSpecimen();

//        if (type != typeof(CertificateAuthorityDataset))
//            return new NoSpecimen();

//        var a = DedicatedFileName
//            .ProximityPaymentSystemEnvironment
//            .GetRegisteredApplicationProviderIdentifier();
//        var b = Array.Empty<CaPublicKeyCertificate>();

//        return new CertificateAuthorityDataset(
//            DedicatedFileName
//            .ProximityPaymentSystemEnvironment
//            .GetRegisteredApplicationProviderIdentifier(),
//            Array.Empty<CaPublicKeyCertificate>());
//    }

//    #endregion
//}