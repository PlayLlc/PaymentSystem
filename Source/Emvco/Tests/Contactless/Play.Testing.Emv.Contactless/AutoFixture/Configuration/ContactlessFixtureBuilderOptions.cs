namespace Play.Testing.Emv.Contactless.AutoFixture.Configuration;

public class ContactlessFixtureBuilderOptions
{
    public static ContactlessFixtureBuilderOptions Default = new();

    public bool ActivateKernelDbOnInitialization { get; set; } = true;

    public bool InitializeDefaultConfigurationData { get; set; } = true;
}
