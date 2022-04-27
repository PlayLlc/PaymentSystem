﻿using AutoFixture.Kernel;

using Play.Emv.Ber.ValueTypes;

namespace Play.Testing.Emv;

public class SdsSchemeIndicatorBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(SdsSchemeIndicatorBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(SdsSchemeIndicator))
            return new NoSpecimen();

        SdsSchemeIndicator[] all = SdsSchemeIndicator.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}