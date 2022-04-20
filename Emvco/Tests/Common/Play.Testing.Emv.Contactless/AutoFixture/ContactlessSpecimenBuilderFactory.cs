namespace Play.Testing.Emv.Contactless.AutoFixture;

public class ContactlessSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public ContactlessSpecimenBuilderFactory() : base(CreateSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    private static List<SpecimenBuilder> CreateSpecimenBuilders()
    {
        // Add upstream builders
        List<SpecimenBuilder> builders = EmvSpecimenBuilderFactory.CreateSpecimenBuilders();

        // Add context specific SpecimenBuilders here
        builders.AddRange(new List<SpecimenBuilder>() {new CertificateAuthorityDatasetBuilder()});

        return builders;
    }

    #endregion
}