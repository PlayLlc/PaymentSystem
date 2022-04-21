﻿using AutoFixture.Kernel;

using Play.Core.Exceptions;
using Play.Globalization.Language;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

public class Alpha2LanguageCodeBuilder : SpecimenBuilder
{
    #region Static Metadata

    private static readonly List<Language> _Languages = LanguageCodeRepository.GetAll();
    public static readonly SpecimenBuilderId Id = new(nameof(Alpha2LanguageCodeBuilder));

    #endregion

    #region Instance Members

    public override SpecimenBuilderId GetId() => Id;

    /// <exception cref="IccProtocolException"></exception>
    /// <exception cref="PlayInternalException"></exception>
    public override object Create(object request, ISpecimenContext context)
    {
        Type? type = request as Type;

        if (type == null)
            return new NoSpecimen();

        if (type != typeof(Alpha2LanguageCode))
            return new NoSpecimen();

        return _Languages.ElementAt(_Random.Next(0, _Languages.Count - 1)).GetAlpha2Code();
    }

    #endregion
}