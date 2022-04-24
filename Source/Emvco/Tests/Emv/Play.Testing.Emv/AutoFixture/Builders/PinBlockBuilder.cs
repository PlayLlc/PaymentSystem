﻿using AutoFixture.Kernel;

using Play.Emv.Ber.ValueTypes;
using Play.Randoms;

namespace Play.Testing.Emv;

public class PinBlockBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(PinBlockBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(PinBlock))
            return new NoSpecimen();

        return new PinBlock(Randomize.Integers.ULong());
    }

    #endregion
}