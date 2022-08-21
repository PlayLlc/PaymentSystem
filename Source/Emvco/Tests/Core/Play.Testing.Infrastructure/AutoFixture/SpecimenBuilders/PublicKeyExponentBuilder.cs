﻿using AutoFixture.Kernel;

using Play.Core.Exceptions;
using Play.Encryption.Certificates;
using Play.Icc.Exceptions;

namespace Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

public class PublicKeyExponentBuilder : SpecimenBuilder
{
    #region Static Metadata

    private static readonly PublicKeyExponents[] _Values = PublicKeyExponents.Empty.GetAll();
    public static readonly SpecimenBuilderId Id = new(nameof(PublicKeyExponentBuilder));

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

        if (type != typeof(PublicKeyExponents))
            return new NoSpecimen();

        return _Values[_Random.Next(0, _Values.Length - 1)];
    }

    #endregion
}