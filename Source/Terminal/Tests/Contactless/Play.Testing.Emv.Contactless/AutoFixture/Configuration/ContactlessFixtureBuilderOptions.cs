using AutoFixture;

using Play.Ber.DataObjects;

namespace Play.Testing.Emv.Contactless.AutoFixture.Configuration;

public class ContactlessFixtureBuilderOptions
{
    public static ContactlessFixtureBuilderOptions Default = new();

    public PrimitiveValue[] DefaultPrimitiveValues { get; set; } = Array.Empty<PrimitiveValue>();

    public Action<IFixture>? RegisterKernelSpecificFixtureValues { get; set; }
}
