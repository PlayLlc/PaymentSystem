using AutoFixture;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Testing.Emv.Contactless.AutoFixture;
using Play.Testing.Emv.Contactless.AutoFixture.Configuration;

namespace Play.Emv.Kernel2.Tests;

public class FixtureSetup
{
    public static IFixture CreateAndSetupKernel2Fixture()
    {
        ContactlessFixtureBuilderOptions options = new()
        {
            DefaultPrimitiveValues = Kernel2DefaultPrimitveValues,
            RegisterKernelSpecificFixtureValues = SetupKernel2SpecificValues
        };

        var fixture = new ContactlessFixture(options).Create();

        return fixture;
    }

    private static PrimitiveValue[] Kernel2DefaultPrimitveValues =>
    new PrimitiveValue[] { TimeoutValue.Default, CardDataInputCapability.Default, SecurityCapability.Default, OutcomeParameterSet.Default,
        ErrorIndication.Default };

    private static void SetupKernel2SpecificValues(IFixture fixture)
    {
        KernelId kernelId = new(2);
        fixture.Register(() => kernelId);
    }
}
