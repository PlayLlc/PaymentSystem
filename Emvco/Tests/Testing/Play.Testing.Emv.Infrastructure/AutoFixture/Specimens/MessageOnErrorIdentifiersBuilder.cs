﻿using AutoFixture.Kernel;

using Play.Emv.Ber;
using Play.Emv.Ber.Enums;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders.Specimens;

namespace Play.Testing.Emv.Infrastructure.AutoFixture.Specimens;

internal class MessageOnErrorIdentifiersBuilder : SpecimenBuilder
{
    #region Static Metadata

    public static readonly SpecimenBuilderId Id = new(nameof(MessageOnErrorIdentifiersBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="Play.Emv.Ber.Exceptions.DataElementParsingException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(MessageOnErrorIdentifiers))
            return new NoSpecimen();

        MessageIdentifiers[] all = MessageIdentifiers.GetAll();

        return (MessageOnErrorIdentifiers) all[new Random().Next(0, all.Length - 1)];
    }

    #endregion
}