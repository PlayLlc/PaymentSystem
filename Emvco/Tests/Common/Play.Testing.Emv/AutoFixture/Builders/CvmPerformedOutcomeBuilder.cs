﻿using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Testing.Infrastructure;

namespace Play.Testing.Emv.Infrastructure.AutoFixture;

internal class CvmPerformedOutcomeBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(CvmPerformedOutcomeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(CvmPerformedOutcome))
            return new NoSpecimen();

        CvmPerformedOutcome[] all = CvmPerformedOutcome.GetAllValues();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}