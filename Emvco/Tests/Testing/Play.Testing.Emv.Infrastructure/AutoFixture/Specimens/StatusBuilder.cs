﻿using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Emv.Infrastructure.AutoFixture.Specimens;

internal class StatusBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(StatusBuilder));

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

        Status[] all = Status.GetAll();

        return all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}