using System;

using AutoFixture.Kernel;

using Play.Emv.Ber.DataElements;
using Play.Emv.Terminal.Contracts.Messages.Commands;
using Play.Testing;

namespace Play.Emv.Kernel.Tests.TerminalRiskManagerTests.Builders;

public class TerminalRiskManagementCommandBuilder : SpecimenBuilder
{
    public override SpecimenBuilderId GetId() => new(nameof(TerminalRiskManagementCommandBuilder));

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(TerminalRiskManagementCommand))
            return new NoSpecimen();

        AmountAuthorizedNumeric amountAuthorizedNumeric = context.Resolve(typeof(AmountAuthorizedNumeric)) as AmountAuthorizedNumeric;


        return new NoSpecimen();
    }
    
}
