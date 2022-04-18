using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Security;

namespace Play.Testing.Emvddd;

public class Kernel2DatabaseBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(Kernel2DatabaseBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(Status))
            return new NoSpecimen();

        var a = new KernelDatabase()


        Status[] all = Status.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}


public class CertificateAuthorityDatasetBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(CertificateAuthorityDatasetBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(Status))
            return new NoSpecimen();

        var a = new CertificateAuthorityDataset()


        Status[] all = Status.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}


public class CaPublicKeyCertificateBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(CaPublicKeyCertificateBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(Status))
            return new NoSpecimen();

        var a = new CaPublicKeyCertificate()


        Status[] all = Status.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}