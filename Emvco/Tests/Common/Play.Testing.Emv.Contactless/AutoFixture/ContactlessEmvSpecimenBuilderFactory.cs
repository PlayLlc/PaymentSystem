namespace Play.Testing.Emv.Contactless.AutoFixture;

public class ContactlessEmvSpecimenBuilderFactory : SpecimenBuilderFactory
{
    #region Constructor

    public ContactlessEmvSpecimenBuilderFactory() : base(CreateSpecimenBuilders())
    { }

    #endregion

    #region Instance Members

    private static List<SpecimenBuilder> CreateSpecimenBuilders() => EmvSpecimenBuilderFactory.CreateSpecimenBuilders();

    #endregion
}